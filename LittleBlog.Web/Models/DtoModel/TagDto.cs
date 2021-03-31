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
        /// 描述
        /// </summary>
        /// <value></value>
        public string Description { get; set; }
    }
}
