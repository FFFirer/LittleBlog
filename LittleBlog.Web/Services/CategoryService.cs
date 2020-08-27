using LittleBlog.Web.Data;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.DomainModels;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public void Delete(int id)
        {
            using (var transcation = _db.Database.BeginTransaction())
            {
                try
                {
                    var category = _db.Categories.AsNoTracking().FirstOrDefault(c => c.Id.Equals(id));

                    if (category == null)
                    {
                        throw new Exception($"not found category id:{id}");
                    }

                    _db.Categories.Remove(category);

                    // 删除视频分类和文章的关系
                    var articleCategories = _db.ArticleCategories.Where(ac => ac.CategoryId.Equals(id)).ToList();
                    _db.ArticleCategories.RemoveRange(articleCategories);

                    _db.SaveChanges();
                    transcation.Commit();
                }
                catch (Exception ex)
                {
                    transcation.Rollback();
                    throw ex;
                }
            }
        }

        public List<Category> Get()
        {
            return _db.Categories.ToList();
        }

        public Category GetById(int id)
        {
            return _db.Categories.FirstOrDefault(c => c.Id.Equals(id));
        }

        public List<Category> GetSummary()
        {
            var categorySummaries = _db.Categories.FromSqlRaw<Category>("select * from Categories ").ToList();
            return categorySummaries;
        }

        public void Save(Category category)
        {
            if(category.Id == 0)
            {
                category.CreateTime = DateTime.Now;
                category.LastEditTime = DateTime.Now;
                _db.Categories.Add(category);
            }
            else
            {
                category.LastEditTime = DateTime.Now;
                _db.Attach(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            _db.SaveChanges();
        }

        public void SaveArticles(int articleId, int categoryId)
        {
            ArticleCategory articleCategory = _db.ArticleCategories
                .FirstOrDefault(ac => ac.ArticleId.Equals(articleId));

            if(articleCategory == null)
            {
                articleCategory = new ArticleCategory()
                {
                    ArticleId = articleId,
                    CategoryId = categoryId
                };
            }
            else
            {
                articleCategory.CategoryId = categoryId;
            }

            _db.SaveChanges();
        }
    }
}
