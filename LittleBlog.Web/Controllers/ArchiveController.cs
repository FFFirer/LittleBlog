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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ArchiveController : Controller
    {
        private IArticleService _articleService;

        public ArchiveController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        //public async Task<IActionResult> Index(int page = 1)
        //{
        //    ArchiveIndexViewModel viewModel = new ArchiveIndexViewModel()
        //    {
        //        ArchivedArticleList = new List<ArchiveIndexListItemViewModel>()
        //    };

        //    var query = new Models.QueryContext.ListArchiveArticlesQueryContext()
        //    {
        //        Page = page,
        //        PageSize = GlobalConfig.PageSize,
        //        OnlyPublished = true,
        //        IsASC = true,
        //    };

        //    List<Article> archiveArticles = await _articleService.ListArchiveArticlesAsync(query);
        //    archiveArticles.GroupBy(a => a.CreateTime.ToString("yyyy-MM-dd")).ToList().ForEach(grouped =>
        //    {
        //        viewModel.ArchivedArticleList.Add(new ArchiveIndexListItemViewModel()
        //        {
        //            ArchiveDate = grouped.Key,
        //            Articles = grouped.ToList()
        //        });
        //    });
        //    viewModel.PageInfo = new Models.ViewModels.PageInfo(page, GlobalConfig.PageSize, query.Total);
        //    return View(viewModel);
        //}

        //[HttpGet]
        //[Route("Date/{archiveDate}")]
        //public async Task<IActionResult> List(string archiveDate)
        //{
        //    if (string.IsNullOrEmpty(archiveDate))
        //    {
        //        return NotFound();
        //    }
        //    ArchiveListViewModel viewModel = new ArchiveListViewModel();
        //    viewModel.Articles = await _articleService.ListAllArticlesByArchiveDateAsync(archiveDate);
        //    return View(viewModel);
        //}

        //[HttpGet]
        //[Route("Category/{categoryId}")]
        //public async Task<IActionResult> Category(int? categoryId)
        //{
        //    if(categoryId == null)
        //    {
        //        return NotFound();
        //    }

        //    ArchiveListViewModel viewModel = new ArchiveListViewModel();
        //    viewModel.Articles = await _articleService.ListAllArticlesByCategoryAsync((int)categoryId);
        //    return View("List", viewModel);
        //}

        //[HttpGet]
        //[Route("Tag/{tagId}")]
        //public async Task<IActionResult> Tag(int? tagId)
        //{
        //    if(tagId == null)
        //    {
        //        return NotFound();
        //    }

        //    ArchiveListViewModel viewModel = new ArchiveListViewModel();
        //    viewModel.Articles = await _articleService.ListAllArticlesByTagAsync((int)tagId);
        //    return View("List", viewModel);
        //}
    }
}
