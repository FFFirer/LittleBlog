using LittleBlog.Web.Constants;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.ViewModels.Manage;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LittleBlog.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
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
            ManageIndexViewModel viewModel = new ManageIndexViewModel()
            {
                Articles = _articleService.GetArticles(out int total, page, GlobalConfig.PageSize),
                PageInfo = new Models.ViewModels.PageInfo(page, GlobalConfig.PageSize, total)
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Article article = _articleService.GetArticle(id);
            if(article == null)
            {
                article = new Article();
            }
            ArticleEditViewModel viewModel = new ArticleEditViewModel(article);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit([FromForm]ArticleEditViewModel articleEdited)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_articleService.SaveArticle(articleEdited))
                    {
                        return Json(ResultModel.Success());
                    }
                    else
                    {
                        return Json(ResultModel.Fail());
                    }
                }
                catch (System.Exception ex)
                {
                    return Json(ResultModel.Error(ex));
                }
            }
            else
            {
                return Json(ResultModel.Fail());
            }
            //return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit", new ArticleEditViewModel(new Article()));
        }
    }
}
