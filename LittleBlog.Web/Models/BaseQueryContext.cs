using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LittleBlog.Web.Models
{
    public class BaseQueryContext
    {
        public QuerySource Source { get; set; }

        /// <summary>
        /// 检查查询条件的权限
        /// </summary>
        public virtual void CheckPermissions()
        {

        }
    }
}
