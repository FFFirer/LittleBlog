using LittleBlog.Web.Constants;
using LittleBlog.Web.Models;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LittleBlog.Web.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleService _articleService;
        private IAuthorizationService _authorizationService;

        public ArticleController(IArticleService articleService, IAuthorizationService authorizationService)
        {
            _articleService = articleService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            Article article = _articleService.GetArticle(id);

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

            return View(article);
        }
    }
}
