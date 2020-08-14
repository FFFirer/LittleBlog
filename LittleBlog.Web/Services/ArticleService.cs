using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Data;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.ViewModels.Manage;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LittleBlog.Web.Services
{
    public class ArticleService : IArticleService
    {
        private LittleBlogContext db;
        public ArticleService(LittleBlogContext context)
        {
            db = context;
        }

        /// <summary>
        /// 获取所有的文章
        /// </summary>
        /// <returns></returns>
        public List<Article> GetAllArticles()
        {
            return db.Articles.ToList();
        }

        /// <summary>
        /// 分页获取文章列表
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="perPage">每页的数量</param>
        /// <returns></returns>
        public List<Article> GetArticles(out int total, int page = 1, int perPage = 20, bool isPublish = false)
        {
            if (isPublish)
            {
                total = db.Articles.Where(a=>a.IsPublished == true).Count();
                return db.Articles.Where(a => a.IsPublished == true).OrderByDescending(b=>b.CreateTime).Skip((page - 1) * perPage).Take(perPage).ToList();
            }
            else
            {
                total = db.Articles.Count();
                return db.Articles.OrderByDescending(b => b.CreateTime).Skip((page - 1) * perPage).Take(perPage).ToList();
            }
        }

        /// <summary>
        /// 根据文章的作者获取文章，搜索
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public List<Article> GetArticles(string keyword, out int total, int page = 1, int perPage = 20, bool isPublish = false)
        {
            if (isPublish)
            {
                total = db.Articles.Where(a => a.IsPublished == true && (a.Author.Contains(keyword) || a.Title.Contains(keyword))).Count();
                return db.Articles.Where(a => a.IsPublished == true && (a.Author.Contains(keyword) || a.Title.Contains(keyword))).Skip((page - 1) * perPage).Take(perPage).ToList();
            }
            else
            {
                total = db.Articles.Where(a => (a.Author.Contains(keyword) || a.Title.Contains(keyword))).Count();
                return db.Articles.Where(a => (a.Author.Contains(keyword) || a.Title.Contains(keyword))).Skip((page - 1) * perPage).Take(perPage).ToList();
            }
        }

        /// <summary>
        /// 保存文章内容变化
        /// </summary>
        /// <param name="article"></param>
        public void SaveContentChange(int articleId, string articleContent)
        {
            var oldarticle = db.Articles.Where(p => p.Id.Equals(articleId)).FirstOrDefault();
            oldarticle.LastEditTime = DateTime.Now;

        }

        /// <summary>
        /// 根据文章Id获取文章内容
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Article GetArticle(int Id)
        {
            return db.Articles.Where(a => a.Id.Equals(Id)).FirstOrDefault();
        }


        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool SaveArticle(ArticleEditViewModel articleEdited)
        {
            if(articleEdited.Id == 0)
            {
                //create
                Article article = new Article();
                articleEdited.UpdateArticle(ref article);
                db.Articles.Add(article);
            }
            else
            {
                // update
                Article article = db.Articles.Where(a => a.Id.Equals(articleEdited.Id)).FirstOrDefault();
                if(article == null)
                {
                    throw new Exception("更新的文章不存在");
                }
                articleEdited.UpdateArticle(ref article);
                db.Articles.Update(article);
            }
            db.SaveChanges();
            return true;
        }

        /// <summary>
        /// 获取归档的文章列表
        /// </summary>
        /// <param name="total"></param>
        /// <param name="page"></param>
        /// <param name="perpage"></param>
        /// <param name="isPublish"></param>
        /// <param name="isOrder"></param>
        /// <returns></returns>
        public List<Article> GetArchiveArticles(out int total, int page = 1, int perpage = 20, bool isPublish = true, bool isOrder = false)
        {
            if(page < 1)
            {
                throw new ArgumentOutOfRangeException("paeg must greater than 0");
            }

            if(perpage < 1)
            {
                throw new ArgumentOutOfRangeException("perpage must greater than 0");
            }
            if (isPublish)
            {
                total = db.Articles.Where(a => a.IsPublished == true).Count();
                return db.Articles.FromSqlRaw<Article>(@"select *, DATE_FORMAT(CreateTime, '%Y-%m-%d') as ArchiveDate from Articles where IsPublished=1 order by ArchiveDate").Skip((page - 1) * perpage).Take(perpage).ToList();
            }
            else
            {
                total = db.Articles.Where(a => a.IsPublished == true).Count();
                return db.Articles.FromSqlRaw<Article>(@"select *, DATE_FORMAT(CreateTime, '%Y-%m-%d') as ArchiveDate from Articles order by ArchiveDate").Skip((page - 1) * perpage).Take(perpage).ToList();
            }
        }
    }
}