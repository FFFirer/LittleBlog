namespace LittleBlog.Core.Models
{
    public class SystemConfig
    {
        /// <summary>
        /// 基础信息
        /// </summary>
        public WebSiteBaseInfo BaseInfo { get; set; }

        /// <summary>
        /// 备案信息
        /// </summary>
        public WebSiteFiling Filing { get; set; }
    }
}
