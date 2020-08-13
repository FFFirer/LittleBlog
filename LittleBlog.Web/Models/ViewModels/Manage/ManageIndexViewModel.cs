using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.ViewModels.Manage
{
    public class ManageIndexViewModel
    {
        public List<Article> Articles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
