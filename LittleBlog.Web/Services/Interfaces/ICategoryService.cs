using LittleBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models.DtoModel;

namespace LittleBlog.Web.Services.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryDto>> ListAsync();

        /// <summary>
        /// 根据id获取Category
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <returns></returns>
        Task<CategoryDto> GetByIdAsync(int id);

        /// <summary>
        /// 获取文章总体情况
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryDto>> ListSummaryAsync();

        /// <summary>
        /// 保存文章分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task SaveAsync(CategoryDto category);

        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(int id);

        /// <summary>
        /// 保存文章与分类关系
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task SaveArticleToCategoryAsync(int articleId, int categoryId);

        /// <summary>
        /// 获取文章的分类
        /// </summary>
        /// <param name="articleId">文章的Id</param>
        /// <returns></returns>
        Task<CategoryDto> GetCategoryByArticleAsync(int articleId);
    }
}
