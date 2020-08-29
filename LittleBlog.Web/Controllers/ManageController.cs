using LittleBlog.Web.Constants;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.ViewModels.Manage;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LittleBlog.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ManageController : Controller
    {
        private IArticleService _articleService;
        private ICategoryService _categoryService;
        private ITagService _tagService;

        public ManageController(IArticleService articleService, ICategoryService categoryService, ITagService tagService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        public IActionResult Index(int page = 1)
        {
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
            viewModel.Article.ArticleCategory = _categoryService.GetCategoryByArticle(article.Id);
            viewModel.Article.ArticleTags = _tagService.GetTagsByArticle(article.Id);
            ViewData["Categories"] = GetAllCategories();
            ViewData["Tags"] = _tagService.Get();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit([FromForm]ArticleEditViewModel articleEdited)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _articleService.SaveArticle(articleEdited.Article);
                    
                    if(articleEdited.CategoryId > 0)
                    {
                        _categoryService.SaveArticles(articleEdited.Article.Id, articleEdited.CategoryId);
                    }

                    if(articleEdited.TagIds?.Count() > 0)
                    {
                        _tagService.SaveArticleTags(articleEdited.Article.Id, articleEdited.TagIds);
                    }

                    return Json(ResultModel.Success());
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

        /// <summary>
        /// 获取所有的类别组成的下拉列表
        /// </summary>
        /// <param name="selectId"></param>
        /// <returns></returns>
        private List<SelectListItem> GetAllCategories(int selectId = 0)
        {
            var categories = _categoryService.Get();
            List<SelectListItem> listItems = new List<SelectListItem>();
            categories.ForEach(c =>
            {
                listItems.Add(new SelectListItem()
                {
                    Text = c.DisplayName,
                    Value = c.Id.ToString(),
                    Selected = c.Id == selectId
                });
            });

            return listItems;
        }
    }
}
