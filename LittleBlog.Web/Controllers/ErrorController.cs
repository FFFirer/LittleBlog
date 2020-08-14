using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LittleBlog.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LittleBlog.Web.Controllers
{
    public class ErrorController : Controller
    {

        [Route("Error/{statusCode}")]
        public IActionResult Index(string statusCode)
        {
            string Message = "服务器开小差了请稍后再试~";

            ErrorIndexViewModel viewModel = new ErrorIndexViewModel()
            {
                StatusCode = statusCode,
                ErrorMessage = Message
            };

            return View(viewModel);
        }
    }
}
