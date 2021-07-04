using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LittleBlog.Web.Models;

namespace LittleBlog.Web.EXtensions
{
    public static class PagingMapExtension
    {
        public static Paging<TDestination> MapPaging<TSource, TDestination>(this IMapper mapper, Paging<TSource> paging)
        {
            var DestinationPaging = new Paging<TDestination>()
            {
                Rows = mapper.Map<IList<TDestination>>(paging.Rows),
                Total = paging.Total
            };

            return DestinationPaging;
        }
    }
}
