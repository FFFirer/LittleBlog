using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace LittleBlog.Web.Common
{
    public static class AdminNavManage
    {
        public static string Manage => "Manage";

        public static string ArticleManage => "ArticleManage";

        public static string CategoryManage => "CategoryManage";

        public static string TagManage => "TagManage";

        public static string ManageNavClass(ViewContext viewContext) => GetNavClass(viewContext, Manage);
        public static string ArticleManageNavClass(ViewContext viewContext) => GetNavClass(viewContext, ArticleManage);
        public static string CategoryManageNavClass(ViewContext viewContext) => GetNavClass(viewContext, CategoryManage);
        public static string TagManageNavClass(ViewContext viewContext) => GetNavClass(viewContext, TagManage);

        public static string GetNavClass(ViewContext viewContext, string targetController)
        {
            string activeController = viewContext.RouteData.Values["controller"].ToString();

            return string.Equals(activeController, targetController, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
