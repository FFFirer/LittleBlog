using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.ViewModels.Home
{
    public class SearchViewModel
    {
        public string keyword { get; set; }
        public List<Article> SearchedArticles { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
