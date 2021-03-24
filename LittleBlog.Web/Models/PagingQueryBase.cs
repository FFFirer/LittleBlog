using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models
{
    /// <summary>
    /// 分页基础
    /// </summary>
    public class PagingBase
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
    }
}
