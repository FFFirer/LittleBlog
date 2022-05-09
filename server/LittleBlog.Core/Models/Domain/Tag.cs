using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBlog.Core.Models
{
    /// <summary>
    /// 标签信息
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// 标签显示名称
        /// </summary>
        /// <value></value>
        [Key]
        [Required]
        [MaxLength(255)]
        [Display(Name = "显示名称")]
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        [Required]
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }
    }
}