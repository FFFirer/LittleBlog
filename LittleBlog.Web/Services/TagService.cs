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
    public class TagService : ITagService
    {
        private LittleBlogContext _db;

        public TagService(LittleBlogContext context)
        {
            _db = context;
        }

        public async Task DeleteAsync(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var tag = await _db.Tags.FirstOrDefaultAsync(t => t.Id.Equals(id));

                    if (tag == null)
                    {
                        throw new Exception($"没有找到Tag,id:{id}");
                    }

                    _db.Tags.Remove(tag);

                    // 删除标签和文章的关系
                    var articleTags = await _db.ArticleTags
                        .Where(at => at.TagId.Equals(id))
                        .ToListAsync();
                    _db.ArticleTags.RemoveRange(articleTags);

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("删除标签失败", ex);
                }
            }
        }

        public async Task<List<Tag>> ListAsync()
        {
            return await _db.Tags.AsNoTracking().ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _db.Tags.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public async Task<List<Tag>> ListSummaryAsync()
        {
            var tagSummaries = await _db.Tags
                .FromSqlRaw("select * from Tags")
                .AsNoTracking()
                .ToListAsync();
            tagSummaries.ForEach(async t =>
            {
                MySqlParameter TagId = new MySqlParameter("tagId", t.Id);
                t.ArticlesCount = await _db.Articles
                .FromSqlRaw("SELECT * FROM Articles a WHERE EXISTS( SELECT 1 FROM ArticleTags WHERE a.Id=ArticleId AND TagId=@tagId) AND a.IsPublished=1", TagId)
                .CountAsync();
            });
            return tagSummaries;
        }

        public async Task<List<Tag>> ListTagsByArticleAsync(int articleId)
        {
            MySqlParameter ArticleId = new MySqlParameter("articleId", articleId);
            return await _db.Tags
                .FromSqlRaw("SELECT * FROM Tags a WHERE EXISTS( SELECT 1 FROM ArticleTags WHERE TagId=a.Id AND ArticleId=@articleId)", ArticleId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveAsync(Tag tag)
        {
            if(tag.Id == 0)
            {
                tag.CreateTime = DateTime.Now;
                tag.LastEditTime = DateTime.Now;
                _db.Tags.Add(tag);
            }
            else
            {
                var oldTag = await _db.Tags
                    .FirstOrDefaultAsync(a => a.Id.Equals(tag.Id));
                if(oldTag == null)
                {
                    throw new Exception("未找到要更新的标签");
                }

                oldTag.Description = tag.Description;
                oldTag.DisplayName = tag.DisplayName;
                oldTag.LastEditTime = DateTime.Now;
            }

            await _db.SaveChangesAsync();
        }

        public async Task SaveArticleTagsAsync(int articleId, List<int> tagIds)
        {
            var oldArticleTags = await _db.ArticleTags
                .Where(at => at.ArticleId.Equals(articleId)).ToListAsync();
            var waitForAdd = tagIds.Except(oldArticleTags.Select(oat => oat.TagId).ToList());
            var waitForRemove = oldArticleTags.Select(oat => oat.TagId).Except(tagIds).ToList();

            _db.ArticleTags.AddRange(waitForAdd.Select(w => new ArticleTag() { ArticleId = articleId, TagId = w }));
            _db.ArticleTags.RemoveRange(oldArticleTags.Where(oat => waitForRemove.Exists(w=>w.Equals(oat.TagId))));

            await _db.SaveChangesAsync();
        }
    }
}
