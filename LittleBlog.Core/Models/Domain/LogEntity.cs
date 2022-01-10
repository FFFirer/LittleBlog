using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LittleBlog.Core.Models
{
    /// <summary>
    /// 日志实体
    /// </summary>
    public class LogEntity
    {
        public int Id { get; set; }
        public string LogLevel { get; set; }
        public string Message { get; set; }
        public string Logger { get; set; }
        public string Application { get; set; }
        public string Callsite { get; set; }
        public string Exception { get; set; }

        [Column(TypeName = "timestamptz")]
        public DateTime Logged { get; set; }
    }
}
