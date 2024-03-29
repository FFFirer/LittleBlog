﻿using LittleBlog.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public interface IArticleService
    {
        /// <summary>
        /// 获取所有的文章
        /// </summary>
        /// <returns></returns>
        Task<List<ArticleDto>> ListAllAsync();

        /// <summary>
        /// 根据分类获取文章
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        Task<List<ArticleDto>> ListByCategoryAsync(int categoryId);

        /// <summary>
        /// 根据标签获取文章
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Task<List<ArticleDto>> ListByTagAsync(int tagId);

        /// <summary>
        /// 根据归档日期获取文章
        /// </summary>
        /// <param name="archiveDate"></param>
        /// <returns></returns>
        Task<List<ArticleDto>> ListByArchiveDateAsync(string archiveDate);

        /// <summary>
        /// 分页获取文章列表
        /// </summary>
        /// <param name="queryContext">查询参数</param>
        /// <returns></returns>
        Task<Paging<Article>> PageAsync(ListArticlesQueryContext queryContext);

        /// <summary>
        /// 保存文章内容变化
        /// </summary>
        /// <param name="articleId">文章Id</param>
        /// <param name="articleContent">文章内容</param>
        Task SaveContentChangeAsync(int articleId, string articleContent);

        /// <summary>
        /// 根据文章Id获取文章内容
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Article> GetAsync(int Id);

        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="article">要保存的文章</param>
        /// <returns></returns>
        Task SaveAsync(Article article);

        /// <summary>
        /// 获取归档的文章列表
        /// </summary>
        /// <param name="queryContext">查询上下文</param>
        /// <returns></returns>
        Task<Paging<ArticleDto>> PageArchivedAsync(ListArchiveArticlesQueryContext queryContext);

        /// <summary>
        /// 获取文章归档情况
        /// </summary>
        /// <returns></returns>
        Task<List<ArchivedArticlesSummary>> GetArchiveSummariesAsync();

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(int id);
    }
}
