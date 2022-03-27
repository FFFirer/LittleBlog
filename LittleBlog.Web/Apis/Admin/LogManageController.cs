using LittleBlog.Core.Models;
using LittleBlog.Core.Models.QueryContext;
using LittleBlog.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System.ComponentModel;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/Admin/Logs")]
    [ApiController]
    [Description("日志管理相关接口（Admin）")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [OpenApiTag("Admin Logs")]
    [Authorize]
    public class LogManageController : BaseApiController
    {
        private readonly ILogService _logService;

        public LogManageController(ILogger<LogManageController> logger, ILogService logService)
        {
            this._logService = logService;
            this._logger = logger;
        }

        [HttpPost("[action]")]
        public async Task<ResultModel<Paging<LogDto>>> List([FromBody]ListLogQueryContext queryContext)
        {
            try
            {
                var paging = await _logService.PageAsync(queryContext);
                return Success(paging);
            }
            catch (System.Exception ex)
            {
                LogException(ex, "查询日志失败！");
                return Fail<Paging<LogDto>>();
            }
        }
    }
}
