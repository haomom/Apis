using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure
{
    public abstract class BaseRepository<T> where T : class
    {
        private readonly IDbSet<T> _dbSet;

        protected IUnitOfWork UnitOfWork { get; set; }

        protected FinanceLedgerPostingDbContext DataContext => (FinanceLedgerPostingDbContext)UnitOfWork;

        protected BaseRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException("UnitOfWork is null");
            _dbSet = DataContext.Set<T>();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }
        
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IRepositoryQuery<T> Query()
        {
            var helper = new RepositoryQuery<T>(this);
            return helper;
        }

        internal IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null,
            int pageNumber = 0,
            int pageSize = 0)
        {
            IQueryable<T> query = _dbSet;

            includes?.ForEach(i => query.Include(i));

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageNumber != 0 && pageSize != 0)
            {
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }
        
            return query.ToList();
        }
    }
}