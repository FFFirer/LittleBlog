using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.ViewModels.Archive
{
    public class ArchiveIndexListItemViewModel
    {
        public string ArchiveDate { get; set; }
        public List<Article> Articles { get; set; }
    }
}
