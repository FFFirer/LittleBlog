using LittleBlog.Web.Common;
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

        public ArticleEditViewModel(Article article)
        {
            if(article.Id == 0)
            {
                this.Author = GlobalConfig.AuthorName;
            }
            else
            {
                this.Author = article.Author;
            }

            this.Id = article.Id;
            this.Title = article.Title;
            this.Content = article.Content;
            this.IsPublished = article.IsPublished;
        }

        public void UpdateArticle(ref Article article)
        {
            if(article == null)
            {
                article = new Article();
            }

            if(article.Id == 0)
            {
                article.CreateTime = DateTime.Now;
            }

            if(this.Title != article.Title)
            {
                article.Title = this.Title;
            }

            if(this.Author != article.Author)
            {
                article.Author = this.Author;
            }

            if(this.Content != article.Content)
            {
                article.Content = this.Content;
            }
            article.Abstract = TextHelper.GetAbstract(this.Content).ToString();

            if (this.IsPublished != article.IsPublished)
            {
                article.IsPublished = this.IsPublished;
            }

            article.LastEditTime = DateTime.Now;
            article.SavePath = string.Empty;
        }

        /// <summary>
        /// 文章Id
        /// </summary>
        /// <value></value>
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }

        /// <summary>
        /// 文章标题
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(255)]
        [Display(Name = "标题")]
        public string Title { get; set; }

        /// <summary>
        /// 文章作者
        /// </summary>
        /// <value></value>
        [Required]
        [MaxLength(255)]
        [Display(Name = "作者")]
        public string Author { get; set; }

        /// <summary>
        /// 文章正文
        /// </summary>
        [Required]
        [Display(Name = "文章正文")]
        public string Content { get; set; }

        /// <summary>
        /// 是否发布
        /// </summary>
        [Display(Name = "状态")]
        public bool IsPublished { get; set; }
    }
}
