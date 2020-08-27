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
    public class TagService : ITagService
    {
        private LittleBlogContext _db;

        public TagService(LittleBlogContext context)
        {
            _db = context;
        }

        public void Delete(int id)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var tag = _db.Tags.FirstOrDefault(t => t.Id.Equals(id));

                    if (tag == null)
                    {
                        throw new Exception($"没有找到Tag,id:{id}");
                    }

                    _db.Tags.Remove(tag);

                    // 删除标签和文章的关系
                    var articleTags = _db.ArticleTags.Where(at => at.TagId.Equals(id)).ToList();
                    _db.ArticleTags.RemoveRange(articleTags);

                    _db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        public List<Tag> Get()
        {
            return _db.Tags.ToList();
        }

        public Tag GetById(int id)
        {
            return _db.Tags.FirstOrDefault(t => t.Id.Equals(id));
        }

        public List<Tag> GetSummary()
        {
            var tagSummaries = _db.Tags.FromSqlRaw("select * from Tags ").ToList();
            return tagSummaries;
        }

        public void Save(Tag tag)
        {
            if(tag.Id == 0)
            {
                tag.CreateTime = DateTime.Now;
                tag.LastEditTime = DateTime.Now;
                _db.Tags.Add(tag);
            }
            else
            {
                tag.LastEditTime = DateTime.Now;
                _db.Attach(tag).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }

            _db.SaveChanges();
        }

        public void SaveArticleTags(int articleId, List<int> tagIds)
        {
            var oldArticleTags = _db.ArticleTags.Where(at => at.ArticleId.Equals(articleId)).ToList();
            var waitForAdd = tagIds.Except(oldArticleTags.Select(oat => oat.TagId).ToList());
            var waitForRemove = oldArticleTags.Select(oat => oat.TagId).Except(tagIds).ToList();

            _db.ArticleTags.AddRange(waitForAdd.Select(w => new ArticleTag() { ArticleId = articleId, TagId = w }));
            _db.ArticleTags.RemoveRange(oldArticleTags.Where(oat => waitForRemove.Exists(w=>w.Equals(oat.TagId))));

            _db.SaveChanges();
        }
    }
}
