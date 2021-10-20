using AutoMapper;
using LittleBlog.Core.Models;
using LittleBlog.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

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
        private IMapper _mapper { get; set; }

        public ArticlesCommonController(IArticleService articleService, 
            ILogger<ArticlesCommonController> logger, 
            IMapper mapper)
        {
            _logger = logger;
            _articleService = articleService;
            _mapper = mapper;
        }

        [HttpGet("[action]")]
        public async Task<ResultModel<Paging<ArticleDto>>> List([FromQuery]ListArticlesQueryContext queryContext)
        {
            try
            {
                queryContext.Source = QuerySource.Common;
                var articles = await _articleService.ListArticlesAsync(queryContext);
                Paging<ArticleDto> articleDtos = articles.MapTo<ArticleDto>(_mapper);
                return Success(articleDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"查询文章列表出错, query: {SerializeToJson(queryContext)}");
                return Fail<Paging<ArticleDto>>(ex, "查询文章列表出错");
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

                var articleDto = _mapper.Map<ArticleDto>(article);
                return Success(articleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError("获取文章出错",ex);
                return Fail<ArticleDto>("获取文章出错");
            }
        }
    }
}
