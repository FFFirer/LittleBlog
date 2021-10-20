using System.ComponentModel.DataAnnotations;

namespace LittleBlog.Core.Models
{
    public class ArticleTag
    {
        [Required]
        public string TagName { get; set; }

        [Required]
        public int ArticleId { get; set; }
    }
}
