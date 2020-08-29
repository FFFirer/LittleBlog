using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.ViewModels
{
    public class ArticleIndexViewModel
    {
        public ArticleIndexViewModel()
        {
            Tags = new List<Tag>();
        }

        public Article Article { get; set; }
        public Category Category { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
