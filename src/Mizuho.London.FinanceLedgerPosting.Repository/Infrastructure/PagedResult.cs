using System;
using System.Collections.Generic;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure
{
    public class PagedResult<T> : IPagedResult<T>
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int RowCount { get; set; }
        
        public int PageCount
        {
            get
            {
                const int minimumPageCount = 1;
                var pageCount = (int)Math.Ceiling(RowCount / (double)PageSize);
                return pageCount >= minimumPageCount ? pageCount : minimumPageCount;
            }
        }
        
        public bool HasNextPage => PageCount > 1 && PageNumber < PageCount;

        public bool HasPreviousPage => PageCount > 1 && PageNumber > 1;

        public IEnumerable<T> PagedList { get; set; }

        public PagedResult()
        {
            PageNumber = 1;
            PageSize = 20;
        }

        public PagedResult(IEnumerable<T> list, int rowCount)
            : this()
        {
            PagedList = list;
            RowCount = rowCount;
        }

        public PagedResult(IEnumerable<T> list, int rowCount, int pageNumber)
            : this(list, rowCount)
        {
            PageNumber = pageNumber;
        }

        public PagedResult(IEnumerable<T> list, int rowCount, int pageNumber, int pageSize)
            : this(list, rowCount, pageNumber)
        {
            PageSize = pageSize;
        }
    }
}
