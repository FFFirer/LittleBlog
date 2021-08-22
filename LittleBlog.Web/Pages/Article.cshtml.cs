using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LittleBlog.Web.Models.DtoModel;
using LittleBlog.Web.Services.Interfaces;
using AutoMapper;

namespace LittleBlog.Web.Pages
{
    public class ArticleModel : PageModel
    {
        public ArticleDto Article { get; set; }

        public IArticleService _service { get; set; }
        public IMapper _mapper { get; set; }

        public ArticleModel(IArticleService articleService)
        {
            _service = articleService;
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
