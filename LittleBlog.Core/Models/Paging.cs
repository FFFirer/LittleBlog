using System.Collections.Generic;
using AutoMapper;

namespace LittleBlog.Core.Models
{
    public class Paging<T>
    {
        public IList<T> Rows { get; set; }

        public int Total { get; set; }

        //public Tuple<IList<T>, int> Result()
        //{
        //    return new Tuple<IList<T>, int>(RowData, Total);
        //}

        public Paging<TDestination> MapTo<TDestination>(IMapper mapper)
        {
            var DestinationPaging = new Paging<TDestination>()
            {
                Rows = mapper.Map<IList<TDestination>>(this.Rows),
                Total = this.Total
            };

            return DestinationPaging;
        }
    }
}
