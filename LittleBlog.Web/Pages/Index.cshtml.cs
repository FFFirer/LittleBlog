using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LittleBlog.Web.Models.DtoModel;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.Extensions.Logging;
using LittleBlog.Web.Models.QueryContext;
using AutoMapper;

namespace LittleBlog.Web.Pages
{
    public class IndexModel : PageModel
    {
        public List<ArticleDto> Articles;
        private readonly ILogger<IndexModel> _logger;
        private IArticleService _service { get; set; }
        private readonly IMapper _mapper;

        public IndexModel(IArticleService articleService, ILogger<IndexModel> logger, IMapper mapper)
        {
            _logger = logger;
            _service = articleService;
            _mapper = mapper;
        }

        public async Task OnGetAsync()
        {
            var queryContext = new ListArticlesQueryContext();
            queryContext.Source = QuerySource.Common;
            queryContext.Page = 1;
            queryContext.PageSize = 20;
            queryContext.OnlyPublished = true;

            var rows = (await _service.ListArticlesAsync(queryContext)).Rows;

            Articles = _mapper.Map<List<ArticleDto>>(rows);
        }
    }
}
