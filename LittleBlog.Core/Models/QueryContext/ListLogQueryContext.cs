using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LittleBlog.Core.Models.QueryContext
{
    public class ListLogQueryContext : PagingBaseQueryContext
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Logger { get; set; }
        public string LogLevel { get; set; }
    }
}
