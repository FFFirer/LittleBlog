using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
