using LittleBlog.Core.Models;
using LittleBlog.Core.Models.QueryContext.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// 获取文章分类
        /// </summary>
        /// <returns></returns>
        Task<List<Category>> ListAllAsync();

        /// <summary>
        /// 保存文章分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task SaveAsync(string categoryName);

        /// <summary>
        /// 删除文章分类
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(string categoryName);

        /// <summary>
        /// 分页查询列表
        /// </summary>
        /// <returns></returns>
        Task<Paging<Category>> ListAsync(ListCategoriesQueryContext query);
    }
}
