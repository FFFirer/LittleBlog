using LittleBlog.Web.Constants;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.ViewModels;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LittleBlog.Web.Controllers
{
    [AllowAnonymous]
    public class ArticleController : Controller
    {
        private IArticleService _articleService;
        private IAuthorizationService _authorizationService;
        private ICategoryService _categoryService;
        private ITagService _tagService;

        public ArticleController(IArticleService articleService, IAuthorizationService authorizationService, ICategoryService categoryService, ITagService tagService)
        {
            _articleService = articleService;
            _authorizationService = authorizationService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            Article article = await _articleService.GetArticleAsync(id);

            if(article == null)
            {
                return NotFound();
            }

            // 验证权限，非管理园返回404
            var isAuthorized = await _authorizationService.AuthorizeAsync(User, article, ArticleOperationRequirements.Details);

            if (!isAuthorized.Succeeded)
            {
                return NotFound();
            }

            ArticleIndexViewModel viewModel = new ArticleIndexViewModel();
            viewModel.Article = article;
            viewModel.Category = await _categoryService.GetCategoryByArticleAsync(article.Id);
            viewModel.Tags = await _tagService.ListTagsByArticleAsync(article.Id);
            return View(viewModel);
        }
    }
}
