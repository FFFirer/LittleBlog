using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Data;
using LittleBlog.Web.Models;
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
        public List<Article> GetArticles(int page = 1, int perPage = 20)
        {
            return db.Articles.Skip(page * perPage).Take(perPage).ToList();
        }

        /// <summary>
        /// 根据文章的作者获取文章
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        public List<Article> GetArticles(string author)
        {
            return db.Articles.Where(p => p.Author.Equals(author)).ToList();
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
    }
}