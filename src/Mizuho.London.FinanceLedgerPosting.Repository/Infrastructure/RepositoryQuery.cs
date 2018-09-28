using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure
{
    public class RepositoryQuery<T> : IRepositoryQuery<T> where T : class
    {
        private readonly BaseRepository<T> _repositoryBase;

        private readonly List<Expression<Func<T, object>>> _includeProperties;

        private Expression<Func<T, bool>> _filter;

        private Func<IQueryable<T>, IOrderedQueryable<T>> _orderByQuerable;

        public RepositoryQuery(BaseRepository<T> repositoryBase)
        {
            _repositoryBase = repositoryBase;
            _includeProperties = new List<Expression<Func<T, object>>>();
        }

        public IRepositoryQuery<T> Filter(Expression<Func<T, bool>> filter)
        {
            _filter = filter;
            return this;
        }

        public IRepositoryQuery<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            _orderByQuerable = orderBy;
            return this;
        }

        public IRepositoryQuery<T> Include(Expression<Func<T, object>> expression)
        {
            _includeProperties.Add(expression);
            return this;
        }

        public async Task<IPagedResult<T>> GetPage(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return new PagedResult<T>(Enumerable.Empty<T>(), 0, pageNumber, pageSize);

            var list = await _repositoryBase.Get(
                filter: _filter,
                orderBy: _orderByQuerable,
                includes: _includeProperties,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var totalList = await _repositoryBase.Get(_filter);
            var total = totalList.Count();
            
            return new PagedResult<T>(list, total, pageNumber, pageSize);
        }

        public async Task<int> GetCount()
        {
            var totalList = await _repositoryBase.Get(_filter);
            return totalList.Count();
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _repositoryBase.Get(_filter, _orderByQuerable, _includeProperties);
        }

        public async Task<T> GetFirstOrDefault()
        {
            return await _repositoryBase.GetFirstOrDefault(_filter, _orderByQuerable, _includeProperties);
        }
    }
}
