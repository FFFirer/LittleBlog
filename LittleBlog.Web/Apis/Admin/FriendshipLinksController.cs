using System.Collections.Generic;
using AutoMapper;
using LittleBlog.Core.Models;
using LittleBlog.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/Admin/SysCfg")]
    [ApiController]
    [Description("系统配置相关接口（Admin）")]
    [ApiExplorerSettings(GroupName = "Admin-SysCfg")]
    [OpenApiTags("Admin Systen Configuration")]
    [Authorize]
    public class FriendshipLinksController : BaseApiController
    {
        private ISettingService _settingService;

        public FriendshipLinksController(ISettingService settingService
                            , ILogger<FriendshipLinksController> logger)
        {
            _settingService = settingService;
            _logger = logger;
        }

        [HttpGet("FriendshipLinks")]
        public async Task<ResultModel<List<FriendshipLink>>> List()
        {
            try
            {
                var links = await _settingService.GetFriendshipLinks();
                return Success(links);
            }
            catch (System.Exception ex)
            {
                LogException(ex, "获取友情链接列表出错");
                return Fail<List<FriendshipLink>>();
            }
        }

        [HttpPost("FriendshipLinks")]
        public async Task<ResultModel> Save(IList<FriendshipLink> links)
        {
            try
            {
                await _settingService.SaveFriendshipLinks(links.ToList());
                return Success();
            }
            catch (System.Exception ex)
            {
                LogException(ex, "保存友情链接列表出错");
                return Fail();
            }
        }
    }
}
