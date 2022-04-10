using LittleBlog.Core.Models;
using LittleBlog.Core.Models.Dto;
using LittleBlog.Core.Models.Dto.Settings;
using LittleBlog.Core.Services.Interfaces;
using LittleBlog.Web.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/Admin/MarkdownThemes")]
    [Authorize]
    [Description("Markdown主题配置")]
    [ApiController]
    public class MdThemesController : BaseApiController
    {
        private readonly IMarkdownThemeService _mdThemeService;
        private readonly IMarkdownBasicSettingService _mdBasicService;

        public MdThemesController(ILoggerFactory loggerFactory, IMarkdownThemeService markdownThemeService, IMarkdownBasicSettingService markdownBasicSettingService)
        {
            _logger = loggerFactory.CreateLogger<MdThemesController>();
            _mdThemeService = markdownThemeService;
            _mdBasicService = markdownBasicSettingService;
        }

        [HttpGet("All")]
        public async Task<ResultModel<List<MarkdownThemeSummaryDto>>> GetAll()
        {
            try
            {

                var dtos = await _mdThemeService.GetAllSummariesAsync();
                return Success(dtos, "获取成功");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "获取失败");
                return Fail<List<MarkdownThemeSummaryDto>>(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ResultModel<MarkdownThemeDto>> GetTheme(Guid id)
        {
            try
            {
                var dto = await _mdThemeService.GetByIdAsync(id);

                return Success(dto, "获取成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取{id}失败");
                return Fail<MarkdownThemeDto>(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ResultModel> Save(MarkdownThemeDto theme)
        {
            try
            {
                await _mdThemeService.SaveAsync(theme);

                // 判断是否是默认的
                // 如果是默认的更新默认主题设置
                var currentTheme = await _mdThemeService.GetByIdAsync(theme.Id);
                var basicInfo = await _mdBasicService.GetMarkdownThemeInfo();
                if(basicInfo.DefaultThemeId == currentTheme.Id.ToString())
                {
                    basicInfo.MarkdownStyleUrl = currentTheme.Url;
                }

                if(basicInfo.DefaultCodeBlockThemeId == currentTheme.Id.ToString())
                {
                    basicInfo.CodeBlockStyleUrl = currentTheme.Url;
                }

                await _mdBasicService.SaveMarkdownThemeInfo(basicInfo);

                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存失败");
                return FailWithMessage($"保存失败：{ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<ResultModel<MarkdownThemeInfo>> GetDefault() 
        {
            try
            {
                var info = await _mdBasicService.GetMarkdownThemeInfo();
                return Success(info);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取Markdown主题默认配置失败");
                return Fail<MarkdownThemeInfo>(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ResultModel> SaveDefault(MarkdownThemeInfo themeInfo)
        {
            try
            {
                await _mdBasicService.SaveMarkdownThemeInfo(themeInfo);
                return Success();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"保存Markdown主题默认配置失败");
                return FailWithMessage(ex.Message);
            }
        }

        [HttpPost("[action]/{id}")]
        public async Task<ResultModel> Remove(Guid id)
        {
            try
            {
                await _mdThemeService.RemoveAsnyc(id);
                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除失败");
                return FailWithMessage(ex.Message);
            }
        }

        [HttpPost("[action]/{id}")]
        public async Task<ResultModel> SetDefaultTheme(Guid id)
        {
            var themeInfo = await _mdThemeService.GetByIdAsync(id);
            if(themeInfo == null)
            {
                return FailWithMessage("没有找到这个主题的信息");
            }

            var basicSetting = await _mdBasicService.GetMarkdownThemeInfo();
            basicSetting.DefaultThemeId = themeInfo.Id.ToString();
            basicSetting.MarkdownStyleUrl = themeInfo.Url;

            await _mdBasicService.SaveMarkdownThemeInfo(basicSetting);

            return Success();
        }

        [HttpPost("[action]/{id}")]
        public async Task<ResultModel> SetCodeBlockDefaultTheme(Guid id)
        {
            var themeInfo = await _mdThemeService.GetByIdAsync(id);
            if (themeInfo == null)
            {
                return FailWithMessage("没有找到这个主题的信息");
            }

            var basicSetting = await _mdBasicService.GetMarkdownThemeInfo();
            basicSetting.DefaultCodeBlockThemeId = themeInfo.Id.ToString();
            basicSetting.CodeBlockStyleUrl = themeInfo.Url;

            await _mdBasicService.SaveMarkdownThemeInfo(basicSetting);

            return Success();
        }
    }
}
