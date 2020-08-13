using System;

namespace LittleBlog.Web.Models.ViewModels
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo
    {
        public int PageCount{ get; set; }
        public int CurrentPage{ get; set; }
        public int PerPageCount{ get; set; }
    }
}
