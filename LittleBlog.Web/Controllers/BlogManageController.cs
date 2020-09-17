using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LittleBlog.Web.Controllers
{
    /// <summary>
    /// 博客基础信息设置
    /// </summary>
    public class BlogManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
