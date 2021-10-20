using LittleBlog.Core.Models;
using System.Collections.Generic;



namespace LittleBlog.Web.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public List<Article> ArticleInfos{ get; set; }

        public PageInfo PageInfo{ get; set; }
    }
}
