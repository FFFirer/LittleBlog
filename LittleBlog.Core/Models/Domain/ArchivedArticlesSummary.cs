using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBlog.Core.Models
{
    public class ArchivedArticlesSummary
    {
        [NotMapped]
        public string DisplayArchiveDate { get; set; }
        public string ArchiveDate { get; set; }
        public int ArticlesCount { get; set; }
    }
}
