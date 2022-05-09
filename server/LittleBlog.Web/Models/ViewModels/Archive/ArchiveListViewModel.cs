using LittleBlog.Core.Models;
using System.Collections.Generic;

namespace LittleBlog.Web.Models.ViewModels.Archive
{
    public class ArchiveListViewModel
    {
        public ArchiveListViewModel()
        {
            Articles = new List<Article>();
        }

        public List<Article> Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
