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

        Task SaveAsync(string tagName);

        Task DeleteAsync(string tagName);
    }
}
