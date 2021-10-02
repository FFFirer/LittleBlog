using LittleBlog.Web.Models.DtoModel.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Services.Interfaces
{
    public interface ISettingService
    {
        Task<WebSiteBaseInfo> GetWebSiteBaseInfo();

        Task<WebSiteFiling> GetWebSiteFiling();

        Task SaveAsync<TSetting>(TSetting setting) where TSetting : class, new();
    }
}
