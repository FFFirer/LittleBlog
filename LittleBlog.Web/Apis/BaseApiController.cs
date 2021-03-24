using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models;
using Microsoft.Extensions.Logging;

namespace LittleBlog.Web.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult Success()
        {
            return new JsonResult(ResultModel.Success());
        }

        protected IActionResult Success(object data)
        {
            return new JsonResult(ResultModel.Success(data));
        }

        protected IActionResult Success(object data, string message)
        {
            return new JsonResult(ResultModel.Success(data, message));
        }

        protected IActionResult Fail(string message)
        {
            return new JsonResult(ResultModel.Fail(message));
        }
        protected IActionResult Fail(Exception ex, string message)
        {
            return new JsonResult(ResultModel.Fail(ex, message));
        }

        protected ILogger _logger { get; set; }

        protected void LogInfo(string message)
        {
            _logger?.LogInformation(message);
        }
        
        protected void LogException(Exception exception, string message = "", LogLevel level = LogLevel.Error)
        {
            _logger?.Log(level, exception, message);
        }

        protected void Log(string message, Exception exception = null, LogLevel level = LogLevel.Information)
        {
            _logger.Log(level, exception, message);
        }
    }
}
