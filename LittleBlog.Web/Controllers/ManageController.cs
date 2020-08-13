using LittleBlog.Web.Mock;
using LittleBlog.Web.Models;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LittleBlog.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private IArticleService _articleService;
        public ManageController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public IActionResult Index(int page = 1)
        {
            // TODO: delete mock artocles
            List<Article> articles = MockData.Instance.articles;
            return View(articles);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Article article = _articleService.GetArticle(id);
            if(article == null)
            {
                article = new Article();
            }
            return View(article);
        }

        [HttpPost]
        public IActionResult Edit(Article article)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit");
        }
    }
}
