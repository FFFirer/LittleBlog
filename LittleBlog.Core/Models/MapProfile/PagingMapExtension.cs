using AutoMapper;
using System.Collections.Generic;

namespace LittleBlog.Core.Models
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
