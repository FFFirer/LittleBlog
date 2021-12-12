using System.Collections.Generic;

namespace LittleBlog.Core.Models
{
    public class ArticleDetailDto : ArticleDto
    {
        public ArticleDetailDto(ArticleDto article)
        {
            this.Id = article.Id;
            this.Title = article.Title;
            this.IsPublished = article.IsPublished;
            this.SavePath = article.SavePath;
            this.Abstract = article.Abstract;
            this.Author = article.Author;
            this.Content = article.Content;
            this.MarkdownContent = article.MarkdownContent;
            this.UseMarkdown = article.UseMarkdown;
        }

        public CategoryDto ArticleCategory { get; set; }

        public List<TagDto> ArticleTags { get; set; } = new List<TagDto>();

        
    }
}
