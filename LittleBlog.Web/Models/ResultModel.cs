using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models
{
    public class ResultModel
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public string exceptionMessage { get; set; }

        public static ResultModel Success(object data = null, string message = "Operation Successed")
        {
            return new ResultModel()
            {
                isSuccess = true,
                message = message,
                data = data,
                exceptionMessage = "",
            };
        }

        public static ResultModel Fail(string message = "Operation Failed")
        {
            return new ResultModel()
            {
                isSuccess = false,
                message = message,
                data = "",
                exceptionMessage = ""
            };
        }

        public static ResultModel Fail(Exception ex, string message = "Operation Failed")
        {
            if(ex is BlogException)
            {
                return new ResultModel()
                {
                    isSuccess = false,
                    message = string.IsNullOrEmpty(ex.Message) ? message : ex.Message,
                    data = "",
                    exceptionMessage = ex?.Message ?? ""
                };
            }
            else
            {
                return new ResultModel()
                {
                    isSuccess = false,
                    message = message,
                    data = "",
                    exceptionMessage = ex?.Message ?? ""
                };
            }
        }
    }
}
