using LittleBlog.Core.Models;
using System.Collections.Generic;

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
