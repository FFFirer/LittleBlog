using LittleBlog.Core.Models;
using System.Threading.Tasks;

namespace LittleBlog.Core.Services
{
    public interface ISettingService
    {
        Task<WebSiteBaseInfo> GetWebSiteBaseInfo();

        Task<WebSiteFiling> GetWebSiteFiling();

        Task SaveAsync<TSetting>(TSetting setting) where TSetting : class, new();
    }
}
