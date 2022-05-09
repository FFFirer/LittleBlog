using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Models.Dto
{
    public class MarkdownThemeSummaryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string Url { get; set; }
        public string PhysicalPath { get; set; }
        public string Remark { get; set; }
        // 最后修改实践
        public DateTime? LastEditTime { get; set; }
    }
}
