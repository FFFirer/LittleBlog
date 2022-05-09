using LittleBlog.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public interface ISettingService
    {
        /// <summary>
        /// 获取站点基础信息
        /// </summary>
        /// <returns></returns>
        Task<WebSiteBaseInfo> GetWebSiteBaseInfo();

        /// <summary>
        /// 获取站点备案信息
        /// </summary>
        /// <returns></returns>
        Task<WebSiteFiling> GetWebSiteFiling();

        /// <summary>
        /// 保存配置项，尽量是单个类型，属性是简单类型，不要集合类型
        /// </summary>
        /// <typeparam name="TSetting"></typeparam>
        /// <param name="setting"></param>
        /// <returns></returns>
        Task SaveAsync<TSetting>(TSetting setting) where TSetting : class, new();

        /// <summary>
        /// 获取当前配置的友情链接
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        Task<List<FriendshipLink>> GetFriendshipLinks(params string[] groups);

        /// <summary>
        /// 保存友情链接
        /// </summary>
        /// <param name="links"></param>
        /// <returns></returns>
        Task SaveFriendshipLinks(List<FriendshipLink> links);
    }
}
