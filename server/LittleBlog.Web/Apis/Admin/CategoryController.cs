using LittleBlog.Core.Models;
using LittleBlog.Core.Models.QueryContext.Category;
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

        [HttpPost("[action]")]
        public async Task<ResultModel<Paging<Category>>> ListSummaries(ListCategoriesQueryContext query)
        {
            try
            {
                var pagedCategories = await _categoryService.ListAsync(query);

                return Success(pagedCategories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "分页加载所有分类出错");
                return Fail<Paging<Category>>(ex);
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
