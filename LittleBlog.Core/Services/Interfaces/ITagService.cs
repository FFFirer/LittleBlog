using LittleBlog.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public interface ITagService
    {
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        Task<List<Tag>> ListAllAsync();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        Task SaveAsync(string tagName);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="tagName"></param>
        /// <returns></returns>
        Task DeleteAsync(string tagName);
    }
}
