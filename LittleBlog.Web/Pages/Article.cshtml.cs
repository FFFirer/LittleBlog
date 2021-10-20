using AutoMapper;
using LittleBlog.Core.Models;
using LittleBlog.Core.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace LittleBlog.Web.Pages
{
    public class ArticleModel : PageModel
    {
        public ArticleDto Article { get; set; }

        public IArticleService _service { get; set; }
        public IMapper _mapper { get; set; }

        public ArticleModel(IArticleService articleService, IMapper mapper)
        {
            _service = articleService;
            _mapper = mapper;
        }

        public async Task OnGet(int id)
        {

            var article = await _service.GetArticleAsync(id);

            if (article == null)
            {
                RedirectToPage("/Error/404");
            }

            Article = _mapper.Map<ArticleDto>(article);
        }
    }
}
