using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Common;
using LittleBlog.Web.Data;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.QueryContext;
using LittleBlog.Web.Models.ViewModels.Manage;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using LittleBlog.Web.Models.DtoModel;

namespace LittleBlog.Web.Services
{
    public class ArticleService : IArticleService
    {
        private LittleBlogContext db;
        public ArticleService(LittleBlogContext context)
        {
            db = context;
        }

        public async Task<List<ArticleDto>> ListAllArticlesAsync()
        {
            return await db.Articles.AsNoTracking()
                .Select(a=>new ArticleDto()
                {
                    Id = a.Id,
                    Abstract = a.Abstract,
                    Title = a.Title,
                    Author = a.Author,
                    Content = a.Content,
                    SavePath = a.SavePath
                })
                .ToListAsync();
        }

        public async Task<List<ArticleDto>> ListArticlesAsync(ListArticlesQueryContext queryContext)
        {
            queryContext.CheckPermissions();

            var Query = db.Articles.AsNoTracking();

            if (!string.IsNullOrEmpty(queryContext.Keyword))
            {
                Query = Query
                    .Where(a => a.Title.Contains(queryContext.Keyword) || a.Author.Contains(queryContext.Keyword));
            }

            if (queryContext.OnlyPublished)
            {
                Query = Query.Where(a => a.IsPublished == true);
            }

            Query = Query.OrderByDescending(a => a.CreateTime);

            queryContext.Total = await Query.CountAsync();

            Query = Query.Paging(queryContext);

            return await Query.Select(a => new ArticleDto()
            {
                Id = a.Id,
                Abstract = a.Abstract,
                Title = a.Title,
                Author = a.Author,
                Content = a.Content,
                SavePath = a.SavePath
            }).ToListAsync();
        }

        public async Task SaveContentChangeAsync(int articleId, string articleContent)
        {
            var oldarticle = await db.Articles
                .Where(p => p.Id.Equals(articleId)).FirstOrDefaultAsync();
            if(oldarticle == null)
            {
                throw new BlogException("未找到文章");
            }
            
            oldarticle.LastEditTime = DateTime.Now;
            oldarticle.Content = articleContent;
            await db.SaveChangesAsync();
        }

        public async Task<ArticleDto> GetArticleAsync(int Id)
        {
            return await db.Articles
                .AsNoTracking()
                .Where(a => a.Id.Equals(Id))
                .Select(a => new ArticleDto()
                {
                    Id = a.Id,
                    Abstract = a.Abstract,
                    Title = a.Title,
                    Author = a.Author,
                    Content = a.Content,
                    SavePath = a.SavePath
                })
                .FirstOrDefaultAsync();
        }

        public async Task SaveArticleAsync(ArticleDto article)
        {
            var articleEntity = UpdateArticle(article);
            if(articleEntity.Id == 0)
            {
                db.Articles.Add(articleEntity);
            }
            else
            {
                // update
                Article oldArticle = await db.Articles
                    .Where(a => a.Id.Equals(article.Id))
                    .FirstOrDefaultAsync();
                if(oldArticle == null)
                {
                    throw new BlogException("更新的文章不存在");
                }
                oldArticle.Title = article.Title;
                oldArticle.Author = article.Author;
                oldArticle.Abstract = article.Abstract;
                oldArticle.Content = article.Content;
                oldArticle.SavePath = article.SavePath;
                oldArticle.LastEditTime = DateTime.Now;
                oldArticle.IsPublished = article.IsPublished;
            }
            await db.SaveChangesAsync();
        }

        public async Task<List<ArticleDto>> ListArchiveArticlesAsync(ListArchiveArticlesQueryContext queryContext)
        {
            IQueryable<Article> Query;

            if (queryContext.OnlyPublished)
            {
                Query = db.Articles
                    .FromSqlRaw<Article>("select *, DATE_FORMAT(CreateTime, '%Y-%m') as ArchiveDate from Articles where IsPublished=1 order by ArchiveDate")
                    .AsNoTracking();
            }
            else
            {
                Query = db.Articles
                    .FromSqlRaw<Article>("select *, DATE_FORMAT(CreateTime, '%Y-%m') as ArchiveDate from Articles order by ArchiveDate")
                    .AsNoTracking();
            }

            queryContext.Total = await Query.CountAsync();

            Query = Query.Paging(queryContext);

            return await Query
                .Select(a => new ArticleDto()
                {
                    Id = a.Id,
                    Abstract = a.Abstract,
                    Title = a.Title,
                    Author = a.Author,
                    Content = a.Content,
                    SavePath = a.SavePath
                }).ToListAsync();
        }

        public async Task<List<ArticleDto>> ListAllArticlesByCategoryAsync(int categoryId)
        {
            //var CategoryId = new MySqlParameter("categoryId", categoryId);
            var sqlParameters = new
            {
                categoryId = categoryId
            };
            return await db.Articles
                .FromSqlRaw("SELECT * FROM Articles a WHERE EXISTS( SELECT 1 FROM ArticleCategories WHERE a.Id=ArticleId AND CategoryId=@categoryId) AND a.IsPublished=1", sqlParameters)
                .AsNoTracking()
                .Select(a => new ArticleDto()
                {
                    Id = a.Id,
                    Abstract = a.Abstract,
                    Title = a.Title,
                    Author = a.Author,
                    Content = a.Content,
                    SavePath = a.SavePath
                })
                .ToListAsync();
        }

        public async Task<List<ArticleDto>> ListAllArticlesByTagAsync(int tagId)
        {
            //var TagId = new MySqlParameter("tagId", tagId);
            var sqlParameters = new
            {
                tagId = tagId
            };
            return await db.Articles
                .FromSqlRaw("SELECT * FROM Articles a WHERE EXISTS(SELECT 1 FROM ArticleTags WHERE a.Id=ArticleId AND TagId=@tagId) AND a.IsPublished=1", sqlParameters)
                .AsNoTracking()
                .Select(a => new ArticleDto()
                {
                    Id = a.Id,
                    Abstract = a.Abstract,
                    Title = a.Title,
                    Author = a.Author,
                    Content = a.Content,
                    SavePath = a.SavePath
                })
                .ToListAsync();
        }


        /// <summary>
        /// 获取文章归档情况
        /// </summary>
        /// <returns></returns> 
        public async Task<List<ArchivedArticlesSummary>> GetArchivedArticlesSummariesAsync()
        {
            
            List<ArchivedArticlesSummary> archivedArticlesSummaries = await db.ArchivedArticlesSummaries
                .FromSqlRaw("SELECT DATE_FORMAT(CreateTime, '%Y-%m') AS ArchiveDate,count(1) AS ArticlesCount FROM Articles WHERE IsPublished=true GROUP BY ArchiveDate;")
                .AsNoTracking()
                .ToListAsync();
            archivedArticlesSummaries.ForEach(a =>
            {
                var temps = a.ArchiveDate.Split('-');
                a.DisplayArchiveDate = $"{temps[0]}年{temps[1]}月";
            });
            return archivedArticlesSummaries;
        }


        /// <summary>
        /// 根据归档日期获取文章
        /// </summary>
        /// <param name="archiveDate"></param>
        /// <returns></returns>
        public async Task<List<ArticleDto>> ListAllArticlesByArchiveDateAsync(string archiveDate)
        {
            //var ArchiveDate = new MySqlParameter("archiveDate", archiveDate);
            var sqlParameters = new
            {
                archiveDate = archiveDate
            };
            return await db.Articles
                .FromSqlRaw("SELECT * FROM Articles WHERE DATE_FORMAT(CreateTime, '%Y-%m')=@archiveDate AND IsPublished=1", sqlParameters)
                .AsNoTracking()
                .Select(a => new ArticleDto()
                {
                    Id = a.Id,
                    Abstract = a.Abstract,
                    Title = a.Title,
                    Author = a.Author,
                    Content = a.Content,
                    SavePath = a.SavePath
                })
                .ToListAsync();
        }

        public async Task DeleteArticleAsync(int id)
        {
            var article = await db.Articles.FirstOrDefaultAsync(a => a.Id.Equals(id));
            if (article != null)
            {
                db.Articles.Remove(article);
                await db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("文章不存在");
            }
        }

        #region 私有方法

        /// <summary>
        /// 更新文章
        /// </summary>
        private Article UpdateArticle(ArticleDto articleDto)
        {
            var article = new Article()
            {
                Id = articleDto.Id,
                Title = articleDto.Title,
                Author = articleDto.Author,
                Abstract = articleDto.Abstract,
                Content = articleDto.Content,
                SavePath = articleDto.SavePath,
                IsPublished = articleDto.IsPublished
            };

            if (article.Id == 0)
            {
                article.CreateTime = DateTime.Now;
            }

            article.Abstract = TextHelper.GetAbstract(article.Content).ToString();
            article.LastEditTime = DateTime.Now;
            article.SavePath = string.Empty;

            return article;
        }

        #endregion
    }
}