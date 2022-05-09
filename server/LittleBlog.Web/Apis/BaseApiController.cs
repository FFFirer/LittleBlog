using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using LittleBlog.Core.Models;
using LittleBlog.Web.Filters;

namespace LittleBlog.Web.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiExceptionFilter))]
    [TypeFilter(typeof(AuditActionFilter))]
    public class BaseApiController : ControllerBase
    {

        #region Success 非泛型
        protected ResultModel Success()
        {
            return SuccessWithMessage("Successed");
        }

        protected ResultModel SuccessWithMessage(string message)
        {
            return Success(message);
        }

        protected ResultModel Success(string message)
        {
            return Result(true, message);
        }

        #endregion

        #region Success 泛型
        protected ResultModel<TData> Success<TData>()
        {
            return Success<TData>(default);
        }

        protected ResultModel<TData> Success<TData>(TData data)
        {
            return Success<TData>(data, "Successed");
        }

        protected ResultModel<TData> Success<TData>(TData data, string message)
        {
            return Result<TData>(true, data, message);
        }

        #endregion

        #region Fail 非泛型
        protected ResultModel Fail()
        {
            return FailWithMessage("Failed");
        }
        protected ResultModel FailWithMessage(string message)
        {
            return Fail(null, message);
        }

        protected ResultModel FailByException(Exception exception)
        {
            return Fail(exception, "Failed");
        }

        protected ResultModel Fail(Exception exception, string message)
        {
            return Result(false, message, exception);
        }
        #endregion

        #region Fail 泛型
        protected ResultModel<TData> Fail<TData>()
        {
            return Fail<TData>("Failed");
        }
        protected ResultModel<TData> Fail<TData>(string message)
        {
            return Fail<TData>(null, message);
        }

        protected ResultModel<TData> Fail<TData>(Exception exception)
        {
            return Fail<TData>(exception, "Failed");
        }

        protected ResultModel<TData> Fail<TData>(Exception exception, string message)
        {
            return Result<TData>(false, default, message, exception);
        }

        #endregion

        #region Result 基础
        protected ResultModel Result(bool isSuccess, string message, Exception exception = null)
        {
            var result = new ResultModel()
            {
                IsSuccess = isSuccess,
                Message = message,
            };
            result.SetException(exception);
            return result;
        }

        protected ResultModel<TData> Result<TData>(bool isSuccess, TData data, string message, Exception exception = null)
        {
            var result = new ResultModel<TData>(data)
            {
                IsSuccess = isSuccess,
                Message = message,
            };
            result.SetException(exception);
            return result;
        }
        #endregion

        #region 日志
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
            _logger?.Log(level, exception, message);
        }

        protected string SerializeToJson(object data)
        {
            return JsonSerializer.Serialize(data);
        }
        #endregion
    }
}
