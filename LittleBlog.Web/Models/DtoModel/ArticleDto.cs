using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DtoModel
{
    public class ArticleDto
    {
        public ArticleDto()
        {
            Author = GlobalConfig.AuthorName;
            ArticleTags = new List<TagDto>();
        }

        /// <summary>
        /// 文章Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        /// <value></value>
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

        public CategoryDto ArticleCategory { get; set; }

        public List<TagDto> ArticleTags { get; set; }
    }
}
