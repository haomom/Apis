using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Interfaces
{
    public interface IRepositoryQuery<T> where T : class
    {
        IRepositoryQuery<T> Filter(Expression<Func<T, bool>> filter);
        IRepositoryQuery<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        IRepositoryQuery<T> Include(Expression<Func<T, object>> expression);
        Task<IPagedResult<T>> GetPage(int pageNumber, int pageSize);
        Task<int> GetCount();
        Task<IEnumerable<T>> Get();
        Task<T> GetFirstOrDefault();
    }
}
