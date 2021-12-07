using LittleBlog.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private LittleBlogContext _db;

        public CategoryService(LittleBlogContext context)
        {
            _db = context;
        }

        public async Task DeleteAsync(string categoryName)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(a => a.Name.Equals(categoryName));
            if(category == null)
            {
                throw new BlogException($"未找到分类{categoryName}");
            }

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Category>> ListAllAsync()
        {
            return await _db.Categories.AsNoTracking().ToListAsync();
        }

        public async Task SaveAsync(string categoryName)
        {
            var category = await _db.Categories.FirstOrDefaultAsync(a => a.Name.Equals(categoryName));
            if(category != null)
            {
                throw new BlogException($"已存在分类{categoryName}");
            }

            category = new Category()
            {
                Name = categoryName,
                CreateTime = DateTime.UtcNow,
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
        }
    }
}
