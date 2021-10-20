using System;
using System.ComponentModel.DataAnnotations;

namespace LittleBlog.Core.Models
{
    /// <summary>
    /// 博客访客
    /// </summary>
    public class BlogUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        /// <value></value>
        [Key]
        [Display(Name="ID")]
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(255)]
        [Display(Name="用户名称")]
        public string UserName{ get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        /// <value></value>
        [Required]
        [Display(Name="注册时间")]
        public DateTime ResigterTime{ get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(255)]
        [Display(Name="密码")]
        public string Password{ get; set; }
    }


}