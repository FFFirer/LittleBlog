using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LittleBlog.Core.Models
{
    /// <summary>
    /// 日志实体
    /// </summary>
    public class LogEntity
    {
        public int Id { get; set; }
        public LogLevel LogLevel { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Application { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
        public string Logged { get; set; }
    }
}
