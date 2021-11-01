using AutoMapper;
using LittleBlog.Core.Models;
using LittleBlog.Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LittleBlog.Web.Pages
{
    public class ArticleModel : PageModel
    {
        public ArticleDto Article { get; set; }

        public IArticleService _service { get; set; }
        public IMapper _mapper { get; set; }

        private ILogger _logger { get; set; }

        public ArticleModel(IArticleService articleService, IMapper mapper, ILoggerFactory loggerFactory)
        {
            _service = articleService;
            _mapper = mapper;
            _logger = loggerFactory.CreateLogger<ArticleModel>();
        }

        public async Task OnGet(int id)
        {
            _logger.LogInformation($"∑√Œ Œƒ’¬[{id}]");

            var article = await _service.GetArticleAsync(id);

            if (article == null)
            {
                RedirectToPage("/Error/404");
            }

            Article = _mapper.Map<ArticleDto>(article);
        }
    }
}
