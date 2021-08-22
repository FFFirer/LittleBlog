using LittleBlog.Web.Models;
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
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/Admin/Categories")]
    [Description("分类管理相关端口（Admin）")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [OpenApiTags("Admin Categories")]
    [ApiController]
    [AllowAnonymous]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ILogger<CategoryController> logger
            , ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet("[action]")]
        public async Task<ResultModel<List<string>>> List()
        {
            try
            {
                var categories = (await _categoryService.ListAllAsync()).Select(a => a.Name).ToList();

                return Success(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载所有分类出错");
                return Fail<List<string>>(ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<ResultModel<List<Category>>> ListAll()
        {
            try
            {
                var categories = await _categoryService.ListAllAsync();

                return Success(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载所有分类出错");
                return Fail<List<Category>>(ex);
            }
        }

        [HttpPost("[action]/{categoryName}")]
        public async Task<ResultModel> Save(string categoryName)
        {
            try
            {
                await _categoryService.SaveAsync(categoryName);
                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"保存分类{categoryName}失败");
                return Fail();
            }
        }

        [HttpPost("[action]/{categoryName}")]
        public async Task<ResultModel> Delete(string categoryName)
        {
            try
            {
                await _categoryService.DeleteAsync(categoryName);
                return Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除分类{categoryName}失败");
                return Fail();
            }
        }
    }
}
