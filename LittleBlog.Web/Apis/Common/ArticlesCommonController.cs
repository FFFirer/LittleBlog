using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;
using LittleBlog.Web.Models.QueryContext;
using NSwag.Annotations;
using Microsoft.AspNetCore.Authorization;
using LittleBlog.Web.Models.DtoModel;
using LittleBlog.Web.Models;

namespace LittleBlog.Web.Apis.Common
{
    [Route("api/Articles")]
    [ApiExplorerSettings(GroupName = "Common")]
    [Description("文章相关接口（Common）")]
    [OpenApiTags("Common Articles")]
    [ApiController]
    [AllowAnonymous]
    public class ArticlesCommonController : BaseApiController
    {
        private IArticleService _articleService { get; set; }

        public ArticlesCommonController(IArticleService articleService, ILogger<ArticlesCommonController> logger)
        {
            _logger = logger;
            _articleService = articleService;
        }

        [HttpGet("[action]")]
        public async Task<ResultModel<List<ArticleDto>>> List([FromQuery]ListArticlesQueryContext queryContext)
        {
            try
            {
                queryContext.Source = QuerySource.Common;
                var list = await _articleService.ListArticlesAsync(queryContext);

                return Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"查询文章列表出错, query: {SerializeToJson(queryContext)}");
                return Fail<List<ArticleDto>>(ex, "查询文章列表出错");
            }
        }

        [HttpGet("{id}")]
        public async Task<ResultModel<ArticleDto>> Get(int id)
        {
            try
            {
                var article = await _articleService.GetArticleAsync(id);
                if (!article.IsPublished)
                {
                    return Fail<ArticleDto>("未找到文章");
                }
                return Success(article);
            }
            catch (Exception ex)
            {
                _logger.LogError("获取文章出错",ex);
                return Fail<ArticleDto>("获取文章出错");
            }
        }
    }
}
