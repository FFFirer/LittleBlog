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
        Task<List<Tag>> ListAllAsync();

        Task SaveAsync(string tagName);

        Task DeleteAsync(string tagName);
    }
}
