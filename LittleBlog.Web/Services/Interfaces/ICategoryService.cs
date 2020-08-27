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
        public List<Category> Get();

        /// <summary>
        /// 根据id获取Category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>
        public Category GetById(int id);

        /// <summary>
        /// 获取文章总体情况
        /// </summary>
        /// <param name="TopCount">默认为0，即所有</param>
        /// <returns></returns>
        public List<Category> GetSummary();

        /// <summary>
        /// 保存文章分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public void Save(Category category);

        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id);

        /// <summary>
        /// 保存文章与分类关系
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="articleIds"></param>
        /// <returns></returns>
        public void SaveArticles(int articleId, int categoryId);
    }
}
