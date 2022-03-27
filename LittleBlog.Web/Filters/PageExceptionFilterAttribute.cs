using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LittleBlog.Web.Filters
{
    /// <summary>
    /// 页面异常过滤器
    /// </summary>
    public class PageExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger _logger;

        public PageExceptionFilterAttribute(IHostEnvironment hostEnvironment, ILogger<PageExceptionFilterAttribute> logger)
        {
            _hostEnvironment = hostEnvironment; 
            _logger = logger;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            HandleException(context);
            return base.OnExceptionAsync(context);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="context"></param>
        private void HandleException(ExceptionContext context)
        {
            if (context.ExceptionHandled)
            {
                return;    
            }

            context.ExceptionHandled = true;

            var feature = context.HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            _logger.LogError(context.Exception, "未处理的异常过滤");

        }
    }
}
