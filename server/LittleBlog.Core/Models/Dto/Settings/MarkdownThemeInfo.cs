using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Models.Dto.Settings
{
    public class MarkdownThemeInfo
    {
        public string DefaultThemeId { get; set; }
        public string DefaultCodeBlockThemeId { get; set; }
        public string MarkdownStyleUrl { get; set; }
        public string CodeBlockStyleUrl { get; set; }
    }
}
