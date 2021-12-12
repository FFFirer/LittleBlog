﻿using System;
using System.Collections.Generic;

namespace LittleBlog.Core.Models
{
    public class ArticleDto
    {
        public ArticleDto()
        {
            Author = GlobalConfig.AuthorName;
        }

        /// <summary>
        /// 文章Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章作者
        /// </summary>
        /// <value></value>
        public string Author { get; set; }

        public string Abstract { get; set; }

        public string Content { get; set; }

        /// <summary>
        /// 保存路径
        /// </summary>
        /// <value></value>
        public string SavePath { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime LastEditTime { get; set; }

        /// <summary>
        /// 使用Markdown编辑
        /// </summary>
        public bool UseMarkdown { get; set; }

        /// <summary>
        /// Markdown内容
        /// </summary>
        public string MarkdownContent { get; set; }

        public List<string> Tags { get; set; }

        public string CategoryName { get; set; }
    }
}
