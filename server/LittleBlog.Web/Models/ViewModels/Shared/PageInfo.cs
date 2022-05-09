using System;

namespace LittleBlog.Web.Models.ViewModels
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo
    {
        public PageInfo(int page, int perpage, int total)
        {
            this.PerPageCount = perpage;

            int pagecount = total / perpage;
            if(total % perpage > 0)
            {
                pagecount++;
            }

            this.PageCount = pagecount;
            this.CurrentPage = page;
            this.TotalCount = total;
        }

        public int PageCount{ get; set; }
        public int CurrentPage{ get; set; }
        public int PerPageCount{ get; set; }
        public int TotalCount { get; set; }
        
    }
}
