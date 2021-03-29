using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LittleBlog.Web.Models;
using LittleBlog.Web.Services;
using LittleBlog.Web.Data;
using LittleBlog.Web.Services.Interfaces;
using LittleBlog.Web.Mock;
using LittleBlog.Web.Models.ViewModels.Home;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace LittleBlog.Web.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : Controller
    {
        private IArticleService _articleService;
        private ILogger _logger;

        public HomeController(IArticleService articleService, ILogger<HomeController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        //public async Task<IActionResult> Index(int page = 1)
        //{
        //    var query = new Models.QueryContext.ListArticlesQueryContext()
        //    {
        //        Page = page,
        //        PageSize = GlobalConfig.PageSize,
        //        Source = QuerySource.Common,
        //        OnlyPublished = true,
        //    };
        //    var results = await _articleService.ListArticlesAsync(query);
        //    Models.ViewModels.HomeIndexViewModel viewmodel = new Models.ViewModels.HomeIndexViewModel();
        //    viewmodel.ArticleInfos = results;
        //    viewmodel.PageInfo = new Models.ViewModels.PageInfo(page, GlobalConfig.PageSize, query.Total);
        //    return View(viewmodel);
        //}

        /////// <summary>
        /////// GET,展示修改界面
        /////// </summary>
        /////// <returns></returns>
        ////[HttpGet]
        ////public IActionResult Edit()
        ////{
        ////    return View(new Article());
        ////}

        /////// <summary>
        /////// POST,提交修改
        /////// </summary>
        /////// <param name="model"></param>
        /////// <returns></returns>
        ////[HttpPost]
        ////public IActionResult Edit([FromBody]EditViewModel model)
        ////{
        ////    try
        ////    {
        ////        _articleService.SaveContentChange(model.Info.Id, model.Info.Content);
        ////        return Index();
        ////    }
        ////    catch (System.Exception ex)
        ////    {
        ////        _logger.LogError(ex, "编辑文章");
        ////        // 返回错误页
        ////        return Error();
        ////    }
        ////}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[HttpGet]
        //public async Task<IActionResult> Search(string keyword, int page = 1)
        //{
        //    try
        //    {
        //        var query = new Models.QueryContext.ListArticlesQueryContext()
        //        {
        //            Keyword = keyword,
        //            Page = page,
        //            PageSize = GlobalConfig.PageSize,
        //            Source = QuerySource.Common,
        //            OnlyPublished = true,
        //        };
        //        SearchViewModel viewModel = new SearchViewModel()
        //        {
        //            keyword = keyword,
        //            SearchedArticles = await _articleService.ListArticlesAsync(query),
        //            PageInfo = new Models.ViewModels.PageInfo(page, GlobalConfig.PageSize, query.Total)
        //        };
        //        return View(viewModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"查询：{keyword}");
        //        return RedirectToAction("Index", "Error", new { statusCode = "500" });
        //    }
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
