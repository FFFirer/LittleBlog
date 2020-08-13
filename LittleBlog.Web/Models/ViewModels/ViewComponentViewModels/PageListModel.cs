using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.ViewModels
{
    public class PageListViewModel
    {

        public List<PageListItem> Items { get; set; }
        public PageInfo pageInfo { get; set; }
        public string ActionName { get; set; }
    }
    
    public class PageListItem
    {
        public string ClassName { get; set; }
        public int TargetPage { get; set; }
        public string InnerHtml { get; set; }
    }
}
