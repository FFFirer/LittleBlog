using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DtoModel
{
    public class ArticleDetailDto : ArticleDto
    {
        public ArticleDetailDto()
        {
            ArticleTags = new List<TagDto>();
        }

        public CategoryDto ArticleCategory { get; set; }

        public List<TagDto> ArticleTags { get; set; }
    }
}
