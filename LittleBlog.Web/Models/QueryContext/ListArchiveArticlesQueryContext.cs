using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.QueryContext
{
    public class ListArchiveArticlesQueryContext : PagingBase
    {
        /// <summary>
        /// 是否发布
        /// </summary>
        public bool OnlyPublished { get; set; } = true;

        /// <summary>
        /// 是否正序
        /// </summary>
        public bool IsASC { get; set; } = true;
    }
}
