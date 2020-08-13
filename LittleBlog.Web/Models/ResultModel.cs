using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models
{
    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string ExceptionMessage { get; set; }

        public static ResultModel Success(object data = null, string message = "Successed")
        {
            return new ResultModel()
            {
                IsSuccess = true,
                Message = message,
                Data = data,
                ExceptionMessage = "",
            };
        }

        public static ResultModel Fail(string message = "Failed")
        {
            return new ResultModel()
            {
                IsSuccess = false,
                Message = message,
                Data = "",
                ExceptionMessage = ""
            };
        }

        public static ResultModel Error(Exception ex, string message = "Error")
        {
            return new ResultModel()
            {
                IsSuccess = false,
                Message = message,
                Data = "",
                ExceptionMessage = ex?.Message ?? ""
            };
        }
    }
}
