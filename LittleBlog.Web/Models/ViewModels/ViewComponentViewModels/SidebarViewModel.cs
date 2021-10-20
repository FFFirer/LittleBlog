using LittleBlog.Core.Models;
using System.Collections.Generic;

namespace LittleBlog.Web.Models.ViewModels
{
    public class SidebarViewModel
    {
        public List<TagSummary> tagSummaries { get; set; }
        public List<CategorySummary> categorySummaries { get; set; }

        public List<ArchivedArticlesSummary> archivedArticlesSummaries { get; set; }
    }

    public class TagSummary
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int Count { get; set; }
    }

    public class CategorySummary
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
    }
}
