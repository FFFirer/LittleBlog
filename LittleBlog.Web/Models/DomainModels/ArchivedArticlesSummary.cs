using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models
{
    public class ArchivedArticlesSummary
    {
        public DateTime CreateTime { get; set; }
        public string ArchiveDate { get; set; }
        public int ArticlesCounts { get; set; }
    }
}
