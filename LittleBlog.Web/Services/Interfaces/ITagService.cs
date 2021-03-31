using LittleBlog.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models.DtoModel;

namespace LittleBlog.Web.Services.Interfaces
{
    public interface ITagService
    {
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        Task<List<TagDto>> ListAsync();

        /// <summary>
        /// 根据Id获取标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TagDto> GetByIdAsync(int id);

        /// <summary>
        /// 获取标签的总体情况
        /// </summary>
        /// <returns></returns>
        Task<List<TagSummaryDto>> ListSummaryAsync();

        /// <summary>
        /// 保存标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task SaveAsync(TagDto tag);

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(int id);

        /// <summary>
        /// 保存文章标签关系
        /// </summary>
        /// <param name="articleId"></param>
        /// <param name="tagIds"></param>
        /// <returns></returns>
        Task SaveArticleTagsAsync(int articleId, List<int> tagIds);

        /// <summary>
        /// 获取文章的标签
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        Task<List<TagDto>> ListTagsByArticleAsync(int articleId);
    }
}
