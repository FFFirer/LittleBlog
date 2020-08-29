using LittleBlog.Web.Models;
using LittleBlog.Web.Models.DomainModels;
using LittleBlog.Web.Models.ViewModels.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Services.Interfaces
{
    public interface IArticleService
    {
        /// <summary>
        /// 获取所有的文章
        /// </summary>
        /// <returns></returns>
        List<Article> GetAllArticles();

        /// <summary>
        /// 根据分类获取文章
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        List<Article> GetAllArticlesByCategory(int categoryId);

        /// <summary>
        /// 根据标签获取文章
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        List<Article> GetAllArticlesByTag(int tagId);

        /// <summary>
        /// 根据归档日期获取文章
        /// </summary>
        /// <param name="archiveDate"></param>
        /// <returns></returns>
        List<Article> GetAllArticlesByArchiveDate(string archiveDate);

        /// <summary>
        /// 分页获取文章列表
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="perPage">每页的数量</param>
        /// <returns></returns>
        List<Article> GetArticles(out int total, int page = 1, int perPage = 20, bool isPublish = false);

        /// <summary>
        /// 根据文章的作者获取文章
        /// </summary>
        /// <param name="author"></param>
        /// <returns></returns>
        List<Article> GetArticles(string keyword, out int total, int page = 1, int perPage = 20, bool isPublish = false);

        /// <summary>
        /// 保存文章内容变化
        /// </summary>
        /// <param name="article"></param>
        void SaveContentChange(int articleId, string articleContent);

        /// <summary>
        /// 根据文章Id获取文章内容
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Article GetArticle(int Id);

        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        void SaveArticle(Article articleEdited);

        /// <summary>
        /// 获取归档的文章列表
        /// </summary>
        /// <param name="total"></param>
        /// <param name="page"></param>
        /// <param name="perpage"></param>
        /// <param name="isPublish"></param>
        /// <param name="isOrder"></param>
        /// <returns></returns>
        List<Article> GetArchiveArticles(out int total, int page = 1, int perpage = 20, bool isPublish = true, bool isOrder = false);

        /// <summary>
        /// 获取文章归档情况
        /// </summary>
        /// <returns></returns>
        List<ArchivedArticlesSummary> GetArchivedArticlesSummaries();
    }
}
