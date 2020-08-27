using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.ViewModels.CategoryManage
{
    public class CategoryManageIndexViewModel
    {
        public CategoryManageIndexViewModel()
        {
            Categories = new List<Category>();
        }

        public List<Category> Categories { get; set; }
    }
}
