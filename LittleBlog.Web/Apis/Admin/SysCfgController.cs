using LittleBlog.Web.Models;
using LittleBlog.Web.Models.DtoModel.Settings;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/Admin/SysCfg")]
    [Authorize]
    [Description("系统配置相关接口（Admin）")]
    [ApiExplorerSettings(GroupName = "Admin-SysCfg")]
    [OpenApiTags("Admin Systen Configuration")]
    [ApiController]
    public class SysCfgController : BaseApiController
    {
        private ISettingService _settings { get; set; }

        public SysCfgController(ISettingService settingService
                              , ILogger<SysCfgController> logger)
        {
            this._settings = settingService;
            this._logger = logger;
        }

        [HttpGet("Base")]
        public async Task<ResultModel<SystemConfig>> Get()
        {
            try
            {
                var config = new SystemConfig();
                config.BaseInfo = await _settings.GetWebSiteBaseInfo();
                config.Filing = await _settings.GetWebSiteFiling();

                return Success<SystemConfig>(config);
            }
            catch (Exception ex)
            {
                return Fail<SystemConfig>(ex);
            }
        } 

        [HttpPost("Base")]
        public async Task<ResultModel> Save(SystemConfig config)
        {
            try
            {
                await _settings.SaveAsync(config.BaseInfo);
                await _settings.SaveAsync(config.Filing);

                return Success();
            }
            catch (Exception ex)
            {
                return Fail(ex, "保存失败");
            }
        }
    }
}
