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
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using LittleBlog.Web.EXtensions;

namespace LittleBlog.Web.Apis.Admin
{
    [Route("api/Admin/Articles")]
    [ApiController]
    [Description("文章管理相关端口（Admin）")]
    [ApiExplorerSettings(GroupName = "Admin")]
    [OpenApiTags("Admin Articles")]
    //[AllowAnonymous]
    [Authorize]
    public class ArticlesManageController : BaseApiController
    {
        private IArticleService _articleService;
        private IMapper _mapper;

        public ArticlesManageController(IArticleService articleService, ILogger<ArticlesManageController> logger, IMapper mapper)
        {
            _articleService = articleService;
            _logger = logger;
            _mapper = mapper;
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
        public async Task<ResultModel> Save([FromServices]ICategoryService categoryService, [FromServices]ITagService tagService, ArticleDto dto)
        {
            try
            {
                var article = _mapper.Map<Article>(dto);
                await _articleService.SaveArticleAsync(article);

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
        public async Task<ResultModel<ArticleDto>> Get([FromServices] ICategoryService categoryService, [FromServices] ITagService tagService, int id)
        {
            try
            {
                var article = await _articleService.GetArticleAsync(id);
                if (article == null) return Fail<ArticleDto>("未找到该文章！");

                //var articleDetail = new ArticleDetailDto(article);

                //ArticleEditViewModel viewModel = new ArticleEditViewModel(articleDetail);
                //viewModel.Article.ArticleCategory = await categoryService.GetCategoryByArticleAsync(article.Id);
                //viewModel.Article.ArticleTags = await tagService.ListTagsByArticleAsync(article.Id);

                //if (viewModel.Article.ArticleCategory != null && viewModel.Article.ArticleCategory.Id > 0)
                //{
                //    viewModel.CategoryId = viewModel.Article.ArticleCategory.Id;
                //}

                //if (viewModel.Article.ArticleTags != null && viewModel.Article.ArticleTags.Count() > 0)
                //{
                //    viewModel.TagIds = viewModel.Article.ArticleTags.Select(t => t.Id).ToList();
                //}
                return Success(article, "获取成功");
            }
            catch (Exception ex)
            {
                LogException(ex, $"获取出错, Id: {id}");
                return Fail<ArticleDto>("获取出错");
            }
        }

        /// <summary>
        /// 获取文章的列表（分页）
        /// </summary>
        /// <param name="queryContext">查询条件</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<ResultModel<Paging<ArticleDto>>> List([FromQuery]ListArticlesQueryContext queryContext)
        {
            try
            {
                queryContext.Source = QuerySource.Admin;
                var Articles = await _articleService.ListArticlesAsync(queryContext);
                var ArticleDtos = _mapper.MapPaging<Article, ArticleDto>(Articles);
                return Success<Paging<ArticleDto>>(ArticleDtos);
            }
            catch (Exception ex)
            {
                LogException(ex, $"查询列表出错, query: {JsonSerializer.Serialize(queryContext)}");
                return Fail<Paging<ArticleDto>>("查询出错");
            }
        }
    }
}
