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

namespace LittleBlog.Web.Apis
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : BaseApiController
    {
        private IArticleService _articleService;

        public ArticlesController(IArticleService articleService, ILogger<ArticlesController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        // 删
        [HttpPost("[action]")]
        public async Task<IActionResult> Delete(int id)
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

        
        // 增、改
        [HttpPost("[action]")]
        public async Task<IActionResult> Save([FromServices]ICategoryService categoryService, [FromServices]ITagService tagService, ArticleEditViewModel model)
        {
            try
            {
                await _articleService.SaveArticleAsync(model.Article);
                if(model.CategoryId > 0)
                {
                    await categoryService.SaveArticleToCategoryAsync(model.Article.Id, model.CategoryId);
                }

                if(model.TagIds?.Count() > 0)
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
        
        // 查
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromServices]ICategoryService categoryService,[FromServices]ITagService tagService,int id)
        {
            try
            {
                var article = await _articleService.GetArticleAsync(id);
                if (article == null) return Fail("未找到该文章！");

                ArticleEditViewModel viewModel = new ArticleEditViewModel(article);
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
                return Fail("获取出错");
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> List([FromQuery]ListArticlesQueryContext queryContext)
        {
            try
            {
                var Articles = await _articleService.ListArticlesAsync(queryContext);
                return Success(Articles);  
            }
            catch (Exception ex)
            {
                LogException(ex, $"查询列表出错, query: {JsonSerializer.Serialize(queryContext)}");
                return Fail("查询出错");
            }
        }
    }
}
