using LittleBlog.Core.Models;
using LittleBlog.Core.Services;
using LittleBlog.Web.Filters;
using Microsoft.AspNetCore.Authorization;
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
    [Route("api/Admin/Tags")]
    [Description("标签管理相关端口（Admin）")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [OpenApiTags("Admin Tags")]
    [ApiController]
    [AllowAnonymous]
    public class TagsManageController : BaseApiController
    {
        private ITagService _tagService;

        public TagsManageController(ITagService tagService, LoggerFactory loggerFactory)
        {
            _tagService = tagService;
            _logger = loggerFactory.CreateLogger<TagsManageController>();
        }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultModel<List<string>>> List()
        {
            try
            {
                var tags = (await _tagService.ListAllAsync()).Select(a=>a.Name).ToList();
                return Success(tags);
            }
            catch (Exception ex)
            {
                LogException(ex, "获取标签出错");
                return Fail<List<string>>(ex, "获取标签出错");
            }
        }

        /// <summary>
        /// 删除一个标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]/{tagName}")]
        public async Task<ResultModel> Delete(string tagName)
        {
            try
            {
                await _tagService.DeleteAsync(tagName);
                return Success("删除成功");
            }
            catch (Exception ex)
            {
                LogException(ex, $"删除tag失败:{tagName}");
                return Fail(ex, $"删除tag失败");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost("[action]/{tagName}")]
        public async Task<ResultModel> Save(string tagName)
        {
            try
            {
                await _tagService.SaveAsync(tagName);
                return Success("保存成功");
            }
            catch (Exception ex)
            {
                LogException(ex, "保存tag失败");
                return Fail(ex, "保存tag失败");
            }
        }
    }
}
