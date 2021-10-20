using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Core.Models
{
    /// <summary>
    /// 分页基础
    /// </summary>
    public class PagingBaseQueryContext : BaseQueryContext
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
