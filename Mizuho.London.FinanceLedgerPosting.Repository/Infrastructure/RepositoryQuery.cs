using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mizuho.London.FinanceLedgerPosting.Interfaces.Infrastructure;

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

        public IPagedResult<T> GetPage(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return new PagedResult<T>(Enumerable.Empty<T>(), 0, pageNumber, pageSize);

            var list = _repositoryBase.Get(
                filter: _filter,
                orderBy: _orderByQuerable,
                includes: _includeProperties,
                pageNumber: pageNumber,
                pageSize: pageSize);

            var total = _repositoryBase.Get(_filter).Count();
            
            return new PagedResult<T>(list, total, pageNumber, pageSize);
        }

        public int GetCount()
        {
            return _repositoryBase.Get(_filter).Count();
        }

        public IEnumerable<T> Get()
        {
            return _repositoryBase.Get(_filter, _orderByQuerable, _includeProperties);
        }

        public T GetFirstOrDefault()
        {
            return _repositoryBase.Get(_filter, _orderByQuerable, _includeProperties).FirstOrDefault();
        }
    }
}
