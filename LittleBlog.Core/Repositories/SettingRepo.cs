using LittleBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Core.Repositories
{
    public class SettingRepo : ISettingRepo
    {
        private LittleBlogContext db { get; set; }
        public SettingRepo(LittleBlogContext db)
        {
            this.db = db;
        }

        public async Task<IList<SettingModel>> GetOneAsync(string sectionName)
        {
            return await db.SettingModels.AsNoTracking()
                                         .Where(a => a.Section == sectionName)
                                         .ToListAsync();
        }

        public async Task<long> SaveListAsync(IList<SettingModel> settings)
        {
            var toAdd = settings.Where(a => a.Id <= 0);
            var toUpdate = settings.Where(a => a.Id > 0);

            var olds = await db.SettingModels.Where(a => toUpdate.Select(b => b.Id).Contains(a.Id))
                    .ToListAsync();

            var oldHasUpdated = olds.Join(toUpdate, newone => newone.Id, oldone => oldone.Id, (oldone, newone) =>
                {
                    oldone.Value = newone.Value;
                    oldone.Description = newone.Description;
                    return oldone;
                }).ToList();

            await db.AddRangeAsync(toAdd.ToList());

            return await db.SaveChangesAsync();
        }

        public async Task<IList<SettingModel>> GetListAsync(string sectionName, List<string> subSectionNames)
        {
            return await db.SettingModels.AsNoTracking()
                .Where(a => a.Section == sectionName && subSectionNames.Contains(a.SubSection))
                .ToListAsync();
        }
    }
}
