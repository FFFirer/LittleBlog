using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.ViewModels.Archive
{
    public class ArchiveIndexViewModel
    {
        public ArchiveIndexViewModel()
        {
            ArchivedArticleList = new List<ArchiveIndexListItemViewModel>();
        }
        public List<ArchiveIndexListItemViewModel> ArchivedArticleList { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
