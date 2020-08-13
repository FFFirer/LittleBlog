using LittleBlog.Web.Models;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LittleBlog.Web.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            Article article = _articleService.GetArticle(id);
            return View(article);
        }
    }
}
