using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace LittleBlog.Web.Filters
{
    public class AuditActionFilter : IActionFilter
    {
        private readonly ILogger _logger;
        public AuditActionFilter(ILogger<AuditActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionDescriptor.RouteValues.TryGetValue("action", out var action);
            context.ActionDescriptor.RouteValues.TryGetValue("controller", out var controller);
            

            _logger.LogInformation($"访问审计：controller:{controller ?? "无"}, action:{action ?? "无"}");
        }
    }
}
