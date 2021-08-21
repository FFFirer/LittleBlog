using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBlog.Web.Models
{
    public class Category
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        /// <value></value>
        [Key]
        [Required]
        [MaxLength(255)]
        [Display(Name="文章分类名称")]
        public string Name{ get; set; }

        /// <summary>
        /// 创建的时间
        /// </summary>
        /// <value></value>
        [Required]
        [Display(Name="创建时间")]
        public DateTime CreateTime{ get; set; }
    }
}