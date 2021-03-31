using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models.QueryContext;
using LittleBlog.Web.Models.DtoModel;
using LittleBlog.Web.Models;
using LittleBlog.Web.Services.Interfaces;

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/Admin/Tags")]
    [ApiController]
    [Description("标签管理相关端口（Admin）")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [OpenApiTags("Admin Tags")]
    public class TagsManageController : BaseApiController
    {
        private ITagService _tagService;

        public TagsManageController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultModel<List<TagDto>>> List()
        {
            try
            {
                var tags = await _tagService.ListAsync();
                return new ResultModel<List<TagDto>>(tags);
            }
            catch (Exception ex)
            {
                LogException(ex, "获取标签出错");
                var result = new ResultModel<List<TagDto>>() { Message = "获取标签出错" };
                result.SetException(ex);
                return result;
            }
        }

        /// <summary>
        /// 根据Id获取一个ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetOne")]
        public async Task<ResultModel<TagDto>> Get(int? id)
        {
            try
            {
                var Tag = await _tagService.GetByIdAsync((int)id);

                return Success<TagDto>(Tag);
            }
            catch (Exception ex)
            {
                LogException(ex, $"找不到要编辑的tag,id:{id}");
                return Fail<TagDto>(ex, $"找不到要编辑的tag,id:{id}");
            }
        }

        /// <summary>
        /// 删除一个标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultModel> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return FailWithMessage("未找到要删除的标签");
                }

                await _tagService.DeleteAsync((int)id);
                return SuccessWithMessage("删除成功");
            }
            catch (Exception ex)
            {
                LogException(ex, $"删除tag失败, id:{id}");
                return Fail(ex, $"删除tag失败, id:{id}");
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ActionResult<ResultModel>> Save(TagDto tag)
        {
            try
            {
                await _tagService.SaveAsync(tag);
                return Success();
            }
            catch (Exception ex)
            {
                LogException(ex, "保存tag失败");
                return Fail(ex, "保存tag失败");
            }
        }

        /// <summary>
        /// 创建标签并返回，webapi
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ResultModel<TagDto>> CreateTag(TagDto tag)
        {
            try
            {
                if (tag.Id > 0)
                {
                    return Fail<TagDto>("此标签已创建");
                }
                else
                {
                    await _tagService.SaveAsync(tag);
                    return Success(tag, "创建成功");
                }
            }
            catch (Exception ex)
            {
                LogException(ex, "新建标签失败");
                return Fail<TagDto>(ex, "新建标签失败");
            }
        }
    }
}
