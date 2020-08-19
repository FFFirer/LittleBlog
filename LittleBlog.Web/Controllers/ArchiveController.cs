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

        public IActionResult List(string archiveDate, int page = 1)
        {
            ArchiveListViewModel viewModel = new ArchiveListViewModel();

            return View(viewModel);
        }
    }
}
