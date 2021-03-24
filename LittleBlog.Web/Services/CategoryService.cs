using LittleBlog.Web.Data;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.DomainModels;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private LittleBlogContext _db;

        public CategoryService(LittleBlogContext context)
        {
            _db = context;
        }

        public async Task DeleteAsync(int id)
        {
            using (var transcation = _db.Database.BeginTransaction())
            {
                try
                {
                    var category = await _db.Categories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(c => c.Id.Equals(id));

                    if (category == null)
                    {
                        throw new Exception($"没有找到分类 id:{id}");
                    }

                    _db.Categories.Remove(category);

                    // 删除视频分类和文章的关系
                    var articleCategories = await _db.ArticleCategories
                        .Where(ac => ac.CategoryId.Equals(id)).ToListAsync();
                    _db.ArticleCategories.RemoveRange(articleCategories);

                    await _db.SaveChangesAsync();
                    await transcation.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transcation.RollbackAsync();
                    throw new Exception("删除分类失败，事务回滚", ex);
                }
            }
        }

        public async Task<List<Category>> ListAsync()
        {
            return await _db.Categories.AsNoTracking().ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _db.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id.Equals(id));
        }

        public async Task<Category> GetCategoryByArticleAsync(int articleId)
        {
            MySqlParameter ArticleId = new MySqlParameter("articleId", articleId);
            return await _db.Categories
                .FromSqlRaw("SELECT * FROM Categories a WHERE EXISTS(SELECT 1 FROM ArticleCategories WHERE CategoryId=a.Id AND ArticleId=@articleId)", ArticleId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<List<Category>> ListSummaryAsync()
        {
            var categorySummaries = await _db.Categories
                .FromSqlRaw<Category>("SELECT * FROM Categories ")
                .AsNoTracking()
                .ToListAsync();
            categorySummaries.ForEach(async c =>
            {
                MySqlParameter CategoryId = new MySqlParameter("categoryId", c.Id);
                c.ArticlesCount = await _db.Articles
                    .FromSqlRaw("SELECT * FROM Articles a WHERE EXISTS(SELECT 1 FROM ArticleCategories WHERE a.Id=ArticleId AND CategoryId=@categoryId) AND a.IsPublished=1", CategoryId)
                    .CountAsync();
            });
            return categorySummaries;
        }

        public async Task SaveAsync(Category category)
        {
            if(category.Id == 0)
            {
                category.CreateTime = DateTime.Now;
                category.LastEditTime = DateTime.Now;
                _db.Categories.Add(category);
            }
            else
            {
                var oldCategory = await _db.Categories
                    .FirstOrDefaultAsync(a => a.Id.Equals(category.Id));
                
                if(oldCategory == null)
                {
                    throw new Exception($"未找到要修改的分类, id: {category.Id}");
                }

                oldCategory.DisplayName = category.DisplayName;
                oldCategory.Description = category.Description;
                oldCategory.LastEditTime = DateTime.Now;

            }

            await _db.SaveChangesAsync();
        }

        public async Task SaveArticleToCategoryAsync(int articleId, int categoryId)
        {
            ArticleCategory articleCategory = await _db.ArticleCategories
                .FirstOrDefaultAsync(ac => ac.ArticleId.Equals(articleId));

            if(articleCategory == null)
            {
                articleCategory = new ArticleCategory()
                {
                    ArticleId = articleId,
                    CategoryId = categoryId
                };
                _db.ArticleCategories.Add(articleCategory);
            }
            else
            {
                articleCategory.CategoryId = categoryId;
            }

            await _db.SaveChangesAsync();
        }
    }
}
