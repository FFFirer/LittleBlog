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

        public async Task<IList<SettingModel>> GetListAsync(string sectionName, params string[] subSectionNames)
        {
            var query = db.SettingModels.AsNoTracking().Where(a => a.Section == sectionName);

            if (subSectionNames.Any())
            {
                query = query.Where(a => subSectionNames.Contains(a.SubSection));
            }

            return await query.ToListAsync();
        }

        // public async Task<long> DeleteAsync(IList<SettingModel> list)
        // {
        //     var validList = list.Where(a => a.Id > 0).Select(a => a.Id).ToList();
        //     var current = await db.SettingModels.Where(a => validList.Contains(a.Id)).ToListAsync();

        //     db.RemoveRange(current);

        //     return await db.SaveChangesAsync();
        // }

        public async Task<long> SaveOrDeleteAsync(IList<SettingModel> toSave, IList<SettingModel> toDelete)
        {
            // Save
            if (toSave.Any())
            {
                var toAdd = toSave.Where(a => a.Id <= 0);
                var toUpdate = toSave.Where(a => a.Id > 0);

                var inDB = await db.SettingModels.Where(a => toUpdate.Select(b => b.Id).Contains(a.Id))
                    .ToListAsync();

                var hasUpdated = inDB.Join(toUpdate, newone => newone.Id, oldone => oldone.Id, (oldone, newone) =>
                    {
                        oldone.Value = newone.Value;
                        oldone.Description = newone.Description;
                        return oldone;
                    }).ToList();

                await db.SettingModels.AddRangeAsync(toAdd.ToList());

            }


            // Delete
            if (toDelete.Any())
            {
                var inDB = db.SettingModels.Where(a => toDelete.Select(b => b.Id).Contains(a.Id));

                db.RemoveRange(inDB);

            }

            return await db.SaveChangesAsync();
        }
    }
}
