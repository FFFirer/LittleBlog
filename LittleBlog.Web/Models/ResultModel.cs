using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models
{
    public class ResultModel<TData>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public TData Data { get; set; } = default;
        public string ExceptionMessage { get; set; } = string.Empty;

        public ResultModel() { }

        public ResultModel(TData data)
        {
            Data = data;
        }
        
        public void SetException(Exception exception)
        {
            if (exception is BlogException)
            {
                if (string.IsNullOrEmpty(exception.Message))
                {
                    Message = exception?.Message ?? string.Empty;
                }
            }
            else
            {
                ExceptionMessage = exception?.Message ?? string.Empty;
            }
        }
    }

    public class ResultModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; } = new object();
        public string ExceptionMessage { get; set; } = string.Empty;

        public ResultModel()
        {

        }

        public ResultModel(object data)
        {
            Data = data;
        }

        public void SetException(Exception exception)
        {
            if (exception == null) return;

            if (exception is BlogException)
            {
                if (!string.IsNullOrEmpty(exception.Message))
                {
                    Message = exception?.Message ?? string.Empty;
                }
            }
            else
            {
                ExceptionMessage = exception?.Message ?? string.Empty;
            }
        }
    }
}
