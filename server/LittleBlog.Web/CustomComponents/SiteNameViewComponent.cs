using LittleBlog.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LittleBlog.Web.CustomComponents
{
    [ViewComponent(Name = "SiteName")]
    public class SiteNameViewComponent : ViewComponent
    {
        private ISettingService _settingService;

        public SiteNameViewComponent(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settting = await _settingService.GetWebSiteBaseInfo();

            return View(settting);
        }
    }
}
