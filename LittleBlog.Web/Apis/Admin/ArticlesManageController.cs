using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models.ViewModels.Manage;
using LittleBlog.Web.Models.QueryContext;
using System.Text.Json;
using System.Text.Json.Serialization;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using NSwag.Annotations;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.DtoModel;

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/Admin/Articles")]
    [ApiController]
    [Description("文章管理相关端口（Admin）")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [OpenApiTags("Admin Articles")]
    public class ArticlesManageController : BaseApiController
    {
        private IArticleService _articleService;

        public ArticlesManageController(IArticleService articleService, ILogger<ArticlesManageController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name = "id"> 文章的Id </param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ResultModel> Delete(int id)
        {
            try
            {
                await _articleService.DeleteArticleAsync(id);
                return Success();
            }
            catch (Exception ex)
            {
                LogException(ex, "删除出错");
                return Fail(ex, "删除出错");
            }
        }

        /// <summary>
        /// 保存文章
        /// </summary>
        /// <param name="categoryService"></param>
        /// <param name="tagService"></param>
        /// <param name="model">文章的主要内容</param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<ResultModel> Save([FromServices]ICategoryService categoryService, [FromServices]ITagService tagService, ArticleEditViewModel model)
        {
            try
            {
                await _articleService.SaveArticleAsync(model.Article);
                if (model.CategoryId > 0)
                {
                    await categoryService.SaveArticleToCategoryAsync(model.Article.Id, model.CategoryId);
                }

                if (model.TagIds?.Count() > 0)
                {
                    await tagService.SaveArticleTagsAsync(model.Article.Id, model.TagIds);
                }

                return Success();
            }
            catch (Exception ex)
            {
                LogException(ex, "保存出错");
                return Fail(ex, "保存出错");
            }
        }

        /// <summary>
        /// 获取单个文章的详情
        /// </summary>
        /// <param name="categoryService"></param>
        /// <param name="tagService"></param>
        /// <param name="id">文章的Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ResultModel<ArticleEditViewModel>> Get([FromServices] ICategoryService categoryService, [FromServices] ITagService tagService, int id)
        {
            try
            {
                var article = await _articleService.GetArticleAsync(id);
                if (article == null) return Fail<ArticleEditViewModel>("未找到该文章！");

                ArticleEditViewModel viewModel = new ArticleEditViewModel((ArticleDetailDto)article);
                viewModel.Article.ArticleCategory = await categoryService.GetCategoryByArticleAsync(article.Id);
                viewModel.Article.ArticleTags = await tagService.ListTagsByArticleAsync(article.Id);

                if (viewModel.Article.ArticleCategory != null && viewModel.Article.ArticleCategory.Id > 0)
                {
                    viewModel.CategoryId = viewModel.Article.ArticleCategory.Id;
                }

                if (viewModel.Article.ArticleTags != null && viewModel.Article.ArticleTags.Count() > 0)
                {
                    viewModel.TagIds = viewModel.Article.ArticleTags.Select(t => t.Id).ToList();
                }
                return Success(viewModel, "获取成功");
            }
            catch (Exception ex)
            {
                LogException(ex, $"获取出错, Id: {id}");
                return Fail<ArticleEditViewModel>("获取出错");
            }
        }

        /// <summary>
        /// 获取文章的列表（分页）
        /// </summary>
        /// <param name="queryContext">查询条件</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultModel<List<ArticleDto>>> List([FromQuery] ListArticlesQueryContext queryContext)
        {
            try
            {
                var Articles = await _articleService.ListArticlesAsync(queryContext);
                return Success<List<ArticleDto>>(Articles);
            }
            catch (Exception ex)
            {
                LogException(ex, $"查询列表出错, query: {JsonSerializer.Serialize(queryContext)}");
                return Fail<List<ArticleDto>>("查询出错");
            }
        }
    }
}
