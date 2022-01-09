using LittleBlog.Core.Models;
using LittleBlog.Core.Models.Dto;
using LittleBlog.Core.Models.Dto.Settings;
using LittleBlog.Core.Services.Interfaces;
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

        public MdThemesController(ILoggerFactory loggerFactory, IMarkdownThemeService markdownThemeService)
        {
            _logger = loggerFactory.CreateLogger<MdThemesController>();
            _mdThemeService = markdownThemeService;
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
                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存失败");
                return FailWithMessage($"保存失败：{ex.Message}");
            }
        }

        [HttpGet("[action]")]
        public async Task<ResultModel<MarkdownThemeInfo>> GetDefault([FromServices]IMarkdownBasicSettingService _mdBasicService) 
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
        public async Task<ResultModel> SaveDefault([FromServices] IMarkdownBasicSettingService _mdBasicService, MarkdownThemeInfo themeInfo)
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
    }
}
