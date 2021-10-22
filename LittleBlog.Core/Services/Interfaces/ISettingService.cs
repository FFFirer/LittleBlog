using LittleBlog.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public interface ISettingService
    {
        Task<WebSiteBaseInfo> GetWebSiteBaseInfo();

        Task<WebSiteFiling> GetWebSiteFiling();

        Task SaveAsync<TSetting>(TSetting setting) where TSetting : class, new();

        Task<List<FriendshipLink>> GetFriendshipLinks(params string[] groups);

        Task SaveFriendshipLinks(List<FriendshipLink> links);
    }
}
