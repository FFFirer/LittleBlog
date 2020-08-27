﻿using LittleBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Services.Interfaces
{
    public interface ITagService
    {
        /// <summary>
        /// 获取标签
        /// </summary>
        /// <returns></returns>
        public List<Tag> Get();

        /// <summary>
        /// 根据Id获取标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tag GetById(int id);

        /// <summary>
        /// 获取标签的总体情况
        /// </summary>
        /// <param name="TopCount">默认为0，即所有</param>
        /// <returns></returns>
        public List<Tag> GetSummary();

        /// <summary>
        /// 保存标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public void Save(Tag tag);

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id);

        /// <summary>
        /// 保存文章标签关系
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        public void SaveArticleTags(int articleId, List<int> tagIds);
    }
}
