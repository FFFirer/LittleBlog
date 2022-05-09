using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LittleBlog.Core.Models
{
    public class LogDto
    {
        public int Id { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Application { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }
        public DateTime Logged { get; set; }
    }
}
