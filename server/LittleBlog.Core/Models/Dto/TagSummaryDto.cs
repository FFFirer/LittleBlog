using System.Collections.Generic;

namespace LittleBlog.Core.Models
{
    public class TagSummaryDto : TagDto
    {

        public List<ArticleDto> Articles { get; set; }

        public int ArticlesCount { get; set; }
    }
}
