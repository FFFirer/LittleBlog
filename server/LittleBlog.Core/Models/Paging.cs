using System;
using System.Collections.Generic;
using AutoMapper;

namespace LittleBlog.Core.Models
{
    public class Paging<T>
    {
        private Paging(){}

        public Paging(int pageSize)
        {
            this.PageSize = pageSize;
        }

        public IList<T> Rows { get; set; }

        private int total { get; set; }
        public int Total
        {
            get { return total; }
            set
            {
                total = value;
                _updatePageCount();
            }
        }


        public int PageSize { get; set; }

        public int PageCount { get; set; }

        private void _updatePageCount()
        {
            decimal _mid = this.Total / this.PageSize;
            this.PageCount = (int)Math.Ceiling(_mid);
        }
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
