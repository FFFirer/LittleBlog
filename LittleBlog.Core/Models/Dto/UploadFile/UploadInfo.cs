using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Core.Models
{
    public class UploadInfo
    {
        public string FileName { get; set; }
        public string UploadPath { get; set; }
        public int Index { get; set; }
        public int Total { get; set; }
        public string Group { get; set; }

        /// <summary>
        /// 上传类型，一种类型对应一种保存路径和浏览路径
        /// </summary>
        public string Type { get; set; }
    }
}
