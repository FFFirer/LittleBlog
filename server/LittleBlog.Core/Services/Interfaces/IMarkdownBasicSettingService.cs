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
        /// <summary>
        /// 获取主题
        /// </summary>
        /// <returns></returns>
        Task<MarkdownThemeInfo> GetMarkdownThemeInfo();

        /// <summary>
        /// 保存主题信息
        /// </summary>
        /// <param name="themeInfo"></param>
        /// <returns></returns>
        Task SaveMarkdownThemeInfo(MarkdownThemeInfo themeInfo);
    }
}
