using LittleBlog.Web.Common;
using LittleBlog.Web.Models.DtoModel;
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

        public ArticleEditViewModel(ArticleDetailDto article)
        {
            this.Article = article;
            this.TagIds = new List<int>();
        }

        public ArticleDetailDto Article { get; set; }

        [Display(Name = "文章分类")]
        public int CategoryId { get; set; }

        [Display(Name = "标签")]
        public List<int> TagIds { get; set; }
    }
}
