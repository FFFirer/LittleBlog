using LittleBlog.Web.Models.ViewModels;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.CustomComponents
{
    [ViewComponent(Name = "Sidebar")]
    public class SidebarComponent : ViewComponent
    {
        private ITagService _tagService;
        private ICategoryService _categoryService;
        private IArticleService _articleService;

        public SidebarComponent(ITagService tagService, ICategoryService categoryService, IArticleService articleService)
        {
            _tagService = tagService;
            _categoryService = categoryService;
            _articleService = articleService;
        }

        public IViewComponentResult Invoke()
        {
            SidebarViewModel model = new SidebarViewModel();

            var tagSummaries = _tagService.ListSummaryAsync().Result;
            var categorySummaries = _categoryService.ListSummaryAsync().Result;

            model.tagSummaries = tagSummaries
                .Select(t => new TagSummary() { TagId=t.Id, TagName = t.DisplayName, Count = t.ArticlesCount }).Where(ts => ts.Count > 0).ToList();
            model.categorySummaries = categorySummaries
                .Select(c => new CategorySummary() { CategoryId = c.Id, CategoryName = c.DisplayName, Count = c.ArticlesCount }).Where(cs => cs.Count > 0).ToList();
            model.archivedArticlesSummaries =  _articleService.GetArchivedArticlesSummariesAsync().Result.Where(aas => aas.ArticlesCount > 0).ToList();

            return View(model);
        }
    }
}
