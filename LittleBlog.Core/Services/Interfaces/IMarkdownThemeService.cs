using LittleBlog.Core.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services.Interfaces
{
    public interface IMarkdownThemeService
    {
        /// <summary>
        /// 获取所有主题
        /// </summary>
        /// <param name="includeStyleCss">是否包含样式文件</param>
        /// <returns></returns>
        Task<List<MarkdownThemeDto>> GetAllSync();

        /// <summary>
        /// 获取所有主题概述
        /// </summary>
        /// <returns></returns>
        Task<List<MarkdownThemeSummaryDto>> GetAllSummariesAsync();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="markdownThemeDto"></param>
        /// <returns></returns>
        Task<int> SaveAsync(MarkdownThemeDto markdownThemeDto);

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MarkdownThemeDto> GetByIdAsync(Guid id);

        /// <summary>
        /// 保存到硬盘
        /// </summary>
        /// <param name="themeDto"></param>
        /// <returns></returns>
        Task SaveToDisk(MarkdownThemeDto themeDto);
    }
}
