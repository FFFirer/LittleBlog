using LittleBlog.Core.Models;
using System.Collections.Generic;

namespace LittleBlog.Web.Models.ViewModels.Archive
{
    public class ArchiveIndexListItemViewModel
    {
        public string ArchiveDate { get; set; }
        public List<Article> Articles { get; set; }
    }
}
