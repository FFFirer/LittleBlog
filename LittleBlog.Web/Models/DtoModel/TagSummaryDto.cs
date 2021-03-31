using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DtoModel
{
    public class TagSummaryDto : TagDto
    {

        public List<ArticleDto> Articles { get; set; }

        public int ArticlesCount { get; set; }
    }
}
