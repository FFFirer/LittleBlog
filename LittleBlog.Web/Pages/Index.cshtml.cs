using AutoMapper;
using LittleBlog.Core.Models;
using LittleBlog.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Web.Pages
{
    public class IndexModel : PageModel
    {
        #region ViewModel
        [BindProperty]
        public List<ArticleDto> Articles { get; set; }

        public WebSiteBaseInfo SiteInfo { get; set; }
        #endregion

        #region Services
        private readonly ILogger<IndexModel> _logger;
        private readonly IArticleService _service;
        private readonly IMapper _mapper;
        private readonly ISettingService _settingService;
        #endregion

        public IndexModel(IArticleService articleService, ISettingService settingService, ILogger<IndexModel> logger, IMapper mapper)
        {
            _logger = logger;
            _service = articleService;
            _mapper = mapper;
            _settingService = settingService;
        }

        public async Task OnGetAsync()
        {
            _logger.LogInformation("访问主页");

            SiteInfo = await _settingService.GetWebSiteBaseInfo();
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
