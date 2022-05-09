using LittleBlog.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.CustomComponents
{
    public class FriendshipLinksViewComponent : ViewComponent
    {
        private ISettingService _settingService;


        public FriendshipLinksViewComponent(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var links = await _settingService.GetFriendshipLinks();

            var linksDict = links.GroupBy(a => a.Group)
                .ToDictionary(x => x.Key, y => y.ToList());

            return View(linksDict);
        }
    }
}
