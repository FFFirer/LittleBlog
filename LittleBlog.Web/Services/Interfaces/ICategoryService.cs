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
    }
}
