﻿using System;
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

namespace LittleBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private IArticleService _articleService;

        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index(int page = 1)
        {
            // TODO: modify
            var results = _articleService.GetArticles(out int total, page, GlobalConfig.PageSize, true);
            //var results = MockData.Instance.articles;

            Models.ViewModels.HomeIndexViewModel viewmodel = new Models.ViewModels.HomeIndexViewModel();
            viewmodel.ArticleInfos = results;
            viewmodel.PageInfo = new Models.ViewModels.PageInfo(page, GlobalConfig.PageSize, total);
            return View(viewmodel);
        }

        /// <summary>
        /// GET,展示修改界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit()
        {
            return View(new Article());
        }

        /// <summary>
        /// POST,提交修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit([FromBody]EditViewModel model)
        {
            try
            {
                _articleService.SaveContentChange(model.Info.Id, model.Info.Content);
                return Index();
            }
            catch (System.Exception)
            {
                // 返回错误页
                return Error();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Search(string keyword, int page = 1)
        {
            try
            {
                SearchViewModel viewModel = new SearchViewModel()
                {
                    keyword = keyword,
                    SearchedArticles = _articleService.GetArticles(keyword, out int total, page, GlobalConfig.PageSize, true),
                    PageInfo = new Models.ViewModels.PageInfo(page, GlobalConfig.PageSize, total)
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Error", new { statusCode = "500" });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
