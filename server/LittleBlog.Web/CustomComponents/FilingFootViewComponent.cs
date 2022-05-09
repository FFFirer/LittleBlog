using LittleBlog.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LittleBlog.Web.CustomComponents
{
    [ViewComponent(Name = "FilingFoot")]
    public class FilingFootViewComponent : ViewComponent
    {
        private ISettingService _settingService;

        public FilingFootViewComponent(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var settting = await _settingService.GetWebSiteFiling();

            return View(settting);
        }
    }
}
