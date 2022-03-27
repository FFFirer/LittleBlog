using LittleBlog.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LittleBlog.Web.Filters
{
    /// <summary>
    /// 接口异常过滤器
    /// </summary>
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger _logger;

        public ApiExceptionFilter(IHostEnvironment hostEnvironment, ILogger<ApiExceptionFilter> logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        //public override Task OnExceptionAsync(ExceptionContext context)
        //{
        //    handleException(context);
        //    return Task.CompletedTask;
        //}

        public void OnException(ExceptionContext context)
        {
            handleException(context);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        private void handleException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, $"未处理的异常过滤");

            if (context.ExceptionHandled)
            {
                return;
            }

            var apiResult = new ResultModel()
            {
                IsSuccess = false,
                Message = "服务器内部发生错误",
            };

            if (_hostEnvironment.IsDevelopment())
            {
                apiResult.ExceptionMessage = context.Exception?.ToString(); // 开发模式显示详细信息
            }
            else
            {
                apiResult.ExceptionMessage = context.Exception?.Message;
            }

            context.Result = new JsonResult(apiResult)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
