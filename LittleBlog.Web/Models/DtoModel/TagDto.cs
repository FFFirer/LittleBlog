using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DtoModel
{
    public class TagDto
    {
        /// <summary>
        /// 标签Id
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// 标签显示名称
        /// </summary>
        /// <value></value>
        public string DisplayName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后一次编辑的时间
        /// </summary>
        /// <value></value>
        public DateTime LastEditTime { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        public string Description { get; set; }

        public List<ArticleDto> Articles { get; set; }

        public int ArticlesCount { get; set; }
    }
}
