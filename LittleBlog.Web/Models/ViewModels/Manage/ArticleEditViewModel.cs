using LittleBlog.Web.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.ViewModels.Manage
{
    public class ArticleEditViewModel
    {
        public ArticleEditViewModel()
        {

        }

        public ArticleEditViewModel(Article article)
        {
            this.Article = article;
            this.TagIds = new List<int>();
        }

        public Article Article { get; set; }

        [Display(Name = "文章分类")]
        public int CategoryId { get; set; }

        [Display(Name = "标签")]
        public List<int> TagIds { get; set; }
    }
}
