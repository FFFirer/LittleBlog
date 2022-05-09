using LittleBlog.Core.Models.Domain;
using LittleBlog.Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Repositories
{
    public class MarkdownThemeRepo : IMarkdownThemeRepo
    {
        private readonly LittleBlogContext _db;

        public MarkdownThemeRepo(LittleBlogContext littleBlogContext)
        {
            _db = littleBlogContext;
        }

        public IQueryable<MarkdownTheme> Themes
        {
            get
            {
                return _db.MarkdownThemes.AsNoTracking();
            }
        }

        public async Task<int> CreateAsync(MarkdownTheme theme)
        {
            if (!theme.Id.Equals(Guid.Empty))
            {
                throw new InvalidOperationException("id不为空");
            }

            theme.Id = Guid.NewGuid();
            theme.CreatedTime = DateTime.Now;

            _db.MarkdownThemes.Add(theme);

            return await _db.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var old = await _db.MarkdownThemes.Where(a => a.Id.Equals(id)).FirstOrDefaultAsync();
            if (old == null)
            {
                return 0;
            }

            _db.MarkdownThemes.Remove(old);
            return await _db.SaveChangesAsync();
        }

        public async Task<IList<MarkdownTheme>> GetAllAsync()
        {
            return await _db.MarkdownThemes.OrderBy(a=>a.Name).ToListAsync();
        }

        public async Task<MarkdownTheme> GetOneAsync(Guid id)
        {
            return await _db.MarkdownThemes.AsNoTracking()
                .Where(a=>a.Id.Equals(id))
                .FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(MarkdownTheme theme)
        {
            var old = await _db.MarkdownThemes.Where(a=>a.Id.Equals(theme.Id)).FirstOrDefaultAsync();
            if(old == null)
            {
                return 0;
            }

            old.Name = theme.Name;
            old.PhysicalPath = theme.PhysicalPath;
            old.LastEditTime = theme.LastEditTime;
            old.Url = theme.Url;
            old.LastEditTime = DateTime.Now;
            old.Content = theme.Content;
            old.Remark = theme.Remark;

            return await _db.SaveChangesAsync();
        }
    }
}
