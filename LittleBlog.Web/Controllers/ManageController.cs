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
using System.Threading.Tasks;

namespace LittleBlog.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    [NSwag.Annotations.OpenApiIgnore]
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

        public async Task<IActionResult> Index(int page = 1)
        {
            var query = new Models.QueryContext.ListArticlesQueryContext()
            {
                Page = page,
                PageSize = GlobalConfig.PageSize,
                OnlyPublished = false
            };
            ManageIndexViewModel viewModel = new ManageIndexViewModel()
            {
                Articles = await _articleService.ListArticlesAsync(query),
                PageInfo = new Models.ViewModels.PageInfo(page, GlobalConfig.PageSize, query.Total)
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Article article = await _articleService.GetArticleAsync(id);
            if(article == null)
            {
                article = new Article();
            }
            ArticleEditViewModel viewModel = new ArticleEditViewModel(article);
            viewModel.Article.ArticleCategory = await _categoryService.GetCategoryByArticleAsync(article.Id);
            viewModel.Article.ArticleTags = await _tagService.ListTagsByArticleAsync(article.Id);
            if(viewModel.Article.ArticleCategory != null && viewModel.Article.ArticleCategory.Id > 0)
            {
                viewModel.CategoryId = viewModel.Article.ArticleCategory.Id;
            }

            if(viewModel.Article.ArticleTags != null && viewModel.Article.ArticleTags.Count() > 0)
            {
                viewModel.TagIds = viewModel.Article.ArticleTags.Select(t => t.Id).ToList();
            }
            ViewData["Categories"] = await GetAllCategories();
            ViewData["Tags"] = await _tagService.ListAsync();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit([FromForm]ArticleEditViewModel articleEdited)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _articleService.SaveArticleAsync(articleEdited.Article);
                    
                    if(articleEdited.CategoryId > 0)
                    {
                        _categoryService.SaveArticleToCategoryAsync(articleEdited.Article.Id, articleEdited.CategoryId);
                    }

                    if(articleEdited.TagIds?.Count() > 0)
                    {
                        _tagService.SaveArticleTagsAsync(articleEdited.Article.Id, articleEdited.TagIds);
                    }

                    return Json(ResultModel.Success());
                }
                catch (System.Exception ex)
                {
                    return Json(ResultModel.Fail(ex));
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
            ViewData["Categories"] = GetAllCategories();
            ViewData["Tags"] = _tagService.ListAsync();
            return View("Edit", new ArticleEditViewModel(new Article()));
        }

        [HttpPost("/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _articleService.DeleteArticleAsync(id);
                return Json(ResultModel.Success());
            }
            catch (System.Exception ex)
            {
                return Json(ResultModel.Fail(ex));
            }
        }

        /// <summary>
        /// 获取所有的类别组成的下拉列表
        /// </summary>
        /// <param name="selectId"></param>
        /// <returns></returns>
        private async Task<List<SelectListItem>> GetAllCategories(int selectId = 0)
        {
            var categories = await _categoryService.ListAsync();
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
