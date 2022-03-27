using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Models.Domain
{
    /// <summary>
    /// 访问日志记录
    /// </summary>
    [Table("AccessLogs")]
    public class AccessLogEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime Logged { get; set; }

        /// <summary>
        /// 访问者IP
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 访问链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public string MethodName { get; set; }
    }
}
