using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LittleBlog.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LittleBlog.Web.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {

        [Route("Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            ErrorIndexViewModel viewModel = new ErrorIndexViewModel()
            {
                StatusCode = statusCode
            };

            switch (statusCode)
            {
                case 401:
                    viewModel.ErrorMessage = "您没有权限访问此资源";
                    break;
                case 404:
                    viewModel.ErrorMessage = "您访问的资源不存在或者已删除~";
                    break;
                case 500:
                    viewModel.ErrorMessage = "服务器内部出现了一点小问题~";
                    break;
                default:
                    viewModel.ErrorMessage = "服务器开小差了~请稍后再试~";
                    break;
            }

            return View(viewModel);
        }
    }
}
