﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DtoModel
{
    public class ArticleDetailDto : ArticleDto
    {
        public ArticleDetailDto(ArticleDto article)
        {
            this.Id = article.Id;
            this.Title = article.Title;
            this.IsPublished = article.IsPublished;
            this.SavePath = article.SavePath;
            this.Abstract = article.Abstract;
            this.Author = article.Author;
            this.Content = article.Content;
        }

        public CategoryDto ArticleCategory { get; set; }

        public List<TagDto> ArticleTags { get; set; } = new List<TagDto>();

        
    }
}
