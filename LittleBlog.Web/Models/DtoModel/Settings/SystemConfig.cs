using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models.DtoModel.Settings
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
