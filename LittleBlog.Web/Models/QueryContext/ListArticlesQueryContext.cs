using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models;

namespace LittleBlog.Web.Models.QueryContext
{
    public class ListArticlesQueryContext : PagingBase
    {
        public string Keyword { get; set; }

        // 只包含已发布的
        public bool OnlyPublished { get; set; } = true;
    }
}
