using LittleBlog.Core.Models.Dto.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services.Interfaces
{
    public interface IMarkdownBasicSettingService
    {
        Task<MarkdownThemeInfo> GetMarkdownThemeInfo();
        Task SaveMarkdownThemeInfo(MarkdownThemeInfo themeInfo);
    }
}
