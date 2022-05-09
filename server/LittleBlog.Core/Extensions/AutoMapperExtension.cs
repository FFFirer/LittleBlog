using AutoMapper;
using LittleBlog.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LittleBlog.Core.Extensions
{
    public static class AutoMapperExtension
    {
        public static Paging<TDestination> MapPaging<TSource, TDestination>(this IMapper mapper, Paging<TSource> paging)
        {
            var DestinationPaging = new Paging<TDestination>(paging.PageSize)
            {
                Rows = mapper.Map<IList<TDestination>>(paging.Rows),
                Total = paging.Total
            };

            return DestinationPaging;
        }
    }
}
