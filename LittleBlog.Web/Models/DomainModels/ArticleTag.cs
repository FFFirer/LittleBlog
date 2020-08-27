using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DomainModels
{
    public class ArticleTag
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int TagId { get; set; }
    }
}
