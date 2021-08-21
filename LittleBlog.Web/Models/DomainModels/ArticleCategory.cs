using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DomainModels
{
    public class ArticleCategory
    {
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public int ArticleId { get; set; }
    }
}
