using System.Collections.Generic;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Interfaces
{
    public interface IPagedResult<T>
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int RowCount { get; set; }
        int PageCount { get; }
        bool HasNextPage { get; }
        bool HasPreviousPage { get; }
        IEnumerable<T> PagedList { get; set; }
    }
}
