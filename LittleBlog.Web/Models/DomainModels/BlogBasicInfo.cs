using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace LittleBlog.Web.Models.DomainModels
{
    public class BlogBasicInfo
    {
        #region 博主信息
        /// <summary>
        /// 管理员名称
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string AdminName { get; set; }

        /// <summary>
        /// 管理员邮箱
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string AdminEmail { get; set; }

        /// <summary>
        /// 个人描述
        /// </summary>
        [DataType(DataType.Text)]
        public string Descripton { get; set; }
        #endregion
    }
}
