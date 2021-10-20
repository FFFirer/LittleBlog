using System.ComponentModel.DataAnnotations;

namespace LittleBlog.Core.Models
{
    public class ArticleCategory
    {
        [Required]
        public string CategoryName { get; set; }

        [Required]
        public int ArticleId { get; set; }
    }
}
