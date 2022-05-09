using LittleBlog.Core.Models;
using System.Collections.Generic;

namespace LittleBlog.Web.Models.ViewModels.Manage
{
    public class ManageIndexViewModel
    {
        public List<Article> Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
