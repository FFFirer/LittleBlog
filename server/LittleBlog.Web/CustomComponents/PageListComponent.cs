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
        public IViewComponentResult Invoke(PageInfo pageInfo, string actionName, Dictionary<string,string> routeParams)
        {
            PageListViewModel model = new PageListViewModel();

            model.pageInfo = pageInfo;
            model.ActionName = actionName;
            model.Items = new List<PageListItem>();

            var pageListItem1 = new PageListItem()
            {
                ClassName = pageInfo.CurrentPage == 1 ? "disabled" : "",
                TargetPage = 1,
                InnerHtml = "首页",
                routerParams = new Dictionary<string, string>(routeParams)
            };
            var pageListItem2 = new PageListItem()
            {
                ClassName = pageInfo.CurrentPage == 1 ? "disabled" : "",
                TargetPage = pageInfo.CurrentPage > 1 ? pageInfo.CurrentPage - 1 : 1,
                InnerHtml = "上一页",
                routerParams = new Dictionary<string, string>(routeParams)
            };
            var pageListItem3 = new PageListItem()
            {
                ClassName = pageInfo.CurrentPage >= pageInfo.PageCount ? "disabled" : "",
                TargetPage = pageInfo.CurrentPage >= pageInfo.PageCount ? pageInfo.PageCount : pageInfo.CurrentPage + 1,
                InnerHtml = "下一页",
                routerParams = new Dictionary<string, string>(routeParams)
            };
            var pageListItem4 = new PageListItem()
            {
                ClassName = pageInfo.CurrentPage >= pageInfo.PageCount ? "disabled" : "",
                TargetPage = pageInfo.PageCount,
                InnerHtml = "末页",
                routerParams = new Dictionary<string, string>(routeParams)
            };

            pageListItem1.routerParams.Add("page", pageListItem1.TargetPage.ToString());
            pageListItem2.routerParams.Add("page", pageListItem2.TargetPage.ToString());
            pageListItem3.routerParams.Add("page", pageListItem3.TargetPage.ToString());
            pageListItem4.routerParams.Add("page", pageListItem4.TargetPage.ToString());

            // 首页
            model.Items.Add(pageListItem1);
            // 上一页
            model.Items.Add(pageListItem2);
            // 下一页
            model.Items.Add(pageListItem3);
            // 末页
            model.Items.Add(pageListItem4);
             
            return View(model);
        }
    }
}
