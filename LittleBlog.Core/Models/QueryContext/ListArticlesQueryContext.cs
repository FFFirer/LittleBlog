using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Core.Models;

namespace LittleBlog.Core.Models
{
    public class ListArticlesQueryContext : PagingBaseQueryContext
    {
        public string Keyword { get; set; }

        // 只包含已发布的
        public bool OnlyPublished { get; set; } = true;


        public override void CheckPermissions()
        {
            base.CheckPermissions();

            if(Source != QuerySource.Admin)
            {
                OnlyPublished = true;
            }
        }
    }
}
