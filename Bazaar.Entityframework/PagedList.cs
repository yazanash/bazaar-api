using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bazaar.Entityframework
{
    public class PagedList<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageNumber = pageNumber;
            PageSize= pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        public PagedList<TDestination> MapTo<TDestination>(Func<T, TDestination> mapFunc)
        {
            var mappedItems = Items.Select(mapFunc).ToList();
            return new PagedList<TDestination>(mappedItems, TotalCount, PageNumber, PageSize);
        }
    }
}
