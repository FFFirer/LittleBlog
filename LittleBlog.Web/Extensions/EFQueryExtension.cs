using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models;

namespace LittleBlog.Web
{
    public static class EFQueryExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="paging"></param>
        /// <param name="maxSize">当无分页的时候最大可以取的数量，为0时为无限制</param>
        /// <returns></returns>
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, PagingBase paging, int maxSize = 0)
        {
            if(paging.Page > 0 && paging.PageSize > 0)
            {
                // 正常分页
                return query.Skip((paging.Page - 1) * paging.PageSize)
                    .Take(paging.PageSize);
            }
            else
            {
                if(maxSize > 0)
                {
                    // 限制最大取值
                    return query.Take(maxSize);
                }
                else
                {
                    // 无限制
                    return query;
                }
            }
        }
    }
}
