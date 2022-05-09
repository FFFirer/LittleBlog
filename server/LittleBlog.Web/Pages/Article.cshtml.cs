using AutoMapper;
using LittleBlog.Core.Models;
using LittleBlog.Core.Models.Dto.Settings;
using LittleBlog.Core.Services;
using LittleBlog.Core.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LittleBlog.Web.Pages
{
    public class ArticleModel : PageModel
    {
        public ArticleDto Article { get; set; }

        public MarkdownThemeInfo MdTheme { get; set; }

        public IArticleService _service { get; set; }
        public IMapper _mapper { get; set; }

        private ILogger _logger { get; set; }
        private IMarkdownBasicSettingService _markdownBasicSettingService { get; set; }

        public ArticleModel(IArticleService articleService, IMapper mapper, ILoggerFactory loggerFactory, IMarkdownBasicSettingService markdownBasicSettingService)
        {
            _service = articleService;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<ArticleModel>();
            _markdownBasicSettingService = markdownBasicSettingService;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            _logger.LogInformation($"访问文章[{id}]");

            var article = await _service.GetAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            Article = _mapper.Map<ArticleDto>(article);

            if (Article.UseMarkdown)
            {
                MdTheme = await _markdownBasicSettingService.GetMarkdownThemeInfo();
            }

            return Page();
        }
    }
}
