using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models
{
    public class UploadResult : ResultModel
    {
        /// <summary>
        /// 批量上传时使用区分组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// request url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// temp file name
        /// </summary>
        public string FileId { get; set; }

        /// <summary>
        /// filename after uploaded
        /// </summary>
        public string FileName { get; set; }

        // 表示所有传输已经完成
        public bool IsFinish { get; set; }
    }
}
