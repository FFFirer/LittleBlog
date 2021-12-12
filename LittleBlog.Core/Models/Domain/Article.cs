using System;
using System.ComponentModel.DataAnnotations;

namespace LittleBlog.Core.Models
{

    /// <summary>
    /// 文章信息
    /// </summary>
    public class Article
    {
        public Article()
        {
            Author = GlobalConfig.AuthorName;
        }

        /// <summary>
        /// 文章Id
        /// </summary>
        /// <value></value>
        [Key]
        [Display(Name="ID")]
        public int Id { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(255)]
        [Display(Name="标题")]
        public string Title{ get; set; }

        /// <summary>
        /// 文章作者
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(255)]
        [Display(Name="作者")]
        public string Author{ get; set; }

        [Display(Name ="摘要")]
        public string Abstract { get; set; }

        [Required]
        [Display(Name="文章正文")]
        public string Content{ get; set; }

        /// <summary>
        /// 保存路径
        /// </summary>
        /// <value></value>
        [MaxLength(255)]
        [Display(Name="保存路径")]
        public string SavePath{ get; set; }

        /// <summary>
        /// 编写时间
        /// </summary>
        /// <value></value>
        [Display(Name="创建时间")]
        public DateTime CreateTime{ get; set; }

        /// <summary>
        /// 最后一次修改的时间
        /// </summary>
        /// <value></value>
        [Display(Name="最后一次编辑的时间")]
        public DateTime LastEditTime{ get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        [Display(Name ="状态")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// 使用Markdown编辑
        /// </summary>
        public bool UseMarkDown { get; set; }

        /// <summary>
        /// Markdown内容
        /// </summary>
        public string MarkdownContent { get; set; }

        public string Category { get; set; }
    }
}