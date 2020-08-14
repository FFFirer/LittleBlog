using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using LittleBlog.Web.Models.ViewModels;
using System.Text.Encodings;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace LittleBlog.Web.CustomComponents
{
    /// <summary>
    /// 分页组件
    /// </summary>
    [ViewComponent(Name = "PageList")]
    public class PageListComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PageInfo pageInfo, string actionName, Func<int, object> GetRouteParams)
        {
            PageListViewModel model = new PageListViewModel();

            model.pageInfo = pageInfo;
            model.ActionName = actionName;
            model.Items = new List<PageListItem>();
            // 首页
            model.Items.Add(new PageListItem()
            {
                ClassName = pageInfo.CurrentPage == 1 ? "disabled" : "",
                TargetPage = 1,
                InnerHtml = "首页",
                routerParams = GetRouteParams(1)
            });

            // 上一页
            model.Items.Add(new PageListItem()
            {
                ClassName = pageInfo.CurrentPage == 1 ? "disabled" : "",
                TargetPage = pageInfo.CurrentPage > 1 ? pageInfo.CurrentPage - 1 : 1,
                InnerHtml = "上一页",
                routerParams = GetRouteParams(pageInfo.CurrentPage > 1 ? pageInfo.CurrentPage - 1 : 1)
            });

            // 下一页
            model.Items.Add(new PageListItem()
            {
                ClassName = pageInfo.CurrentPage >= pageInfo.PageCount ? "disabled" : "",
                TargetPage = pageInfo.CurrentPage >= pageInfo.PageCount ? pageInfo.PageCount : pageInfo.CurrentPage + 1,
                InnerHtml = "下一页",
                routerParams = GetRouteParams(pageInfo.CurrentPage >= pageInfo.PageCount ? pageInfo.PageCount : pageInfo.CurrentPage + 1)
            });
            // 末页
            model.Items.Add(new PageListItem()
            {
                ClassName = pageInfo.CurrentPage >= pageInfo.PageCount ? "disabled" : "",
                TargetPage = pageInfo.PageCount,
                InnerHtml = "末页",
                routerParams = GetRouteParams(pageInfo.CurrentPage >= pageInfo.PageCount ? pageInfo.PageCount : pageInfo.CurrentPage + 1)
            });

            return View(model);
        }
    }
}
