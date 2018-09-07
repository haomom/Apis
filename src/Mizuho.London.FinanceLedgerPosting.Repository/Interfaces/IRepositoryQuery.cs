using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Interfaces
{
    public interface IRepositoryQuery<T> where T : class
    {
        IRepositoryQuery<T> Filter(Expression<Func<T, bool>> filter);
        IRepositoryQuery<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        IRepositoryQuery<T> Include(Expression<Func<T, object>> expression);
        IPagedResult<T> GetPage(int pageNumber, int pageSize);
        int GetCount();
        IEnumerable<T> Get();
        T GetFirstOrDefault();
    }
}
