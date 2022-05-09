using LittleBlog.Core.Models;
using System.Collections.Generic;

namespace LittleBlog.Web.Models.ViewModels.Home
{
    public class SearchViewModel
    {
        public string keyword { get; set; }
        public List<Article> SearchedArticles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
