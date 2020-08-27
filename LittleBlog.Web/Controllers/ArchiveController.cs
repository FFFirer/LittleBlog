using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.ViewModels.Archive;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LittleBlog.Web.Controllers
{
    [AllowAnonymous]
    [Route("Archive")]
    public class ArchiveController : Controller
    {
        private IArticleService _articleService;

        public ArchiveController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public IActionResult Index(int page = 1)
        {
            ArchiveIndexViewModel viewModel = new ArchiveIndexViewModel()
            {
                ArchivedArticleList = new List<ArchiveIndexListItemViewModel>()
            };

            List<Article> archiveArticles = _articleService.GetArchiveArticles(out int total, page, GlobalConfig.PageSize, true, true);
            archiveArticles.GroupBy(a => a.CreateTime.ToString("yyyy-MM-dd")).ToList().ForEach(grouped =>
            {
                viewModel.ArchivedArticleList.Add(new ArchiveIndexListItemViewModel()
                {
                    ArchiveDate = grouped.Key,
                    Articles = grouped.ToList()
                });
            });
            viewModel.PageInfo = new Models.ViewModels.PageInfo(page, GlobalConfig.PageSize, total);
            return View(viewModel);
        }

        [HttpGet]
        [Route("Date/{archiveDate}")]
        public IActionResult List(string archiveDate)
        {
            if (string.IsNullOrEmpty(archiveDate))
            {
                return NotFound();
            }
            ArchiveListViewModel viewModel = new ArchiveListViewModel();
            viewModel.Articles = _articleService.GetAllArticlesByArchiveDate(archiveDate);
            return View(viewModel);
        }

        [HttpGet]
        [Route("Category/{categoryId}")]
        public IActionResult Category(int? categoryId)
        {
            if(categoryId == null)
            {
                return NotFound();
            }

            ArchiveListViewModel viewModel = new ArchiveListViewModel();
            viewModel.Articles = _articleService.GetAllArticlesByCategory((int)categoryId);
            return View("List", viewModel);
        }

        [HttpGet]
        [Route("Tag/{tagId}")]
        public IActionResult Tag(int? tagId)
        {
            if(tagId == null)
            {
                return NotFound();
            }

            ArchiveListViewModel viewModel = new ArchiveListViewModel();
            viewModel.Articles = _articleService.GetAllArticlesByTag((int)tagId);
            return View("List", viewModel);
        }
    }
}
