using LittleBlog.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.CustomComponents
{
    [ViewComponent(Name = "Sidebar")]
    public class SidebarComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            SidebarViewModel model = new SidebarViewModel();

            return View(model);
        }
    }
}
