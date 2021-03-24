﻿using LittleBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Services.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <returns></returns>
        Task<List<Category>> ListAsync();

        /// <summary>
        /// 根据id获取Category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>
        Task<Category> GetByIdAsync(int id);

        /// <summary>
        /// 获取文章总体情况
        /// </summary>
        /// <param name="TopCount">默认为0，即所有</param>
        /// <returns></returns>
        Task<List<Category>> ListSummaryAsync();

        /// <summary>
        /// 保存文章分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task SaveAsync(Category category);

        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(int id);

        /// <summary>
        /// 保存文章与分类关系
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="articleIds"></param>
        /// <returns></returns>
        Task SaveArticleToCategoryAsync(int articleId, int categoryId);

        /// <summary>
        /// 获取文章的分类
        /// </summary>
        /// <param name="articleId">文章的Id</param>
        /// <returns></returns>
        Task<Category> GetCategoryByArticleAsync(int articleId);
    }
}
