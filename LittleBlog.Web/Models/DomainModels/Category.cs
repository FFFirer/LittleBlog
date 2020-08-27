using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBlog.Web.Models
{
    public class Category
    {
        /// <summary>
        /// 类别的Id
        /// </summary>
        /// <value></value>
        [Key]
        [Display(Name="ID")]
        public int Id{ get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(255)]
        [Display(Name="文章分类名称")]
        public string DisplayName{ get; set; }

        /// <summary>
        /// 创建的时间
        /// </summary>
        /// <value></value>
        [Required]
        [Display(Name="创建时间")]
        public DateTime CreateTime{ get; set; }
        
        /// <summary>
        /// 最后一次编辑的时间
        /// </summary>
        /// <value></value>
        [Required]
        [Display(Name="最后一次编辑的时间")]
        public DateTime LastEditTime{ get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(255)]
        [Display(Name="描述")]
        public string Description{ get; set; }

        [NotMapped]
        public List<Article> Articles { get; set; }

        [NotMapped]
        public int ArticlesCount { get; set; }
    }
}