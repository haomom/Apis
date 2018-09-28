using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure
{
    public abstract class BaseRepository<T> where T : class
    {
        private readonly DbSet<T> _dbSet;

        protected IUnitOfWork UnitOfWork { get; set; }

        protected FinanceLedgerPostingDbContext DataContext => (FinanceLedgerPostingDbContext)UnitOfWork;

        protected BaseRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _dbSet = DataContext.Set<T>();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T Add(T entity)
        {
            return _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
        }

        public void SetEntityStateModified(T entity)
        {
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public async Task<T> GetById(object id)
        {
            return await _dbSet.FindAsync(id);
        }
        
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public async Task<IEnumerable<T>> GetAllEntities()
        {
            return await _dbSet.ToListAsync();
        }

        public IRepositoryQuery<T> Query()
        {
            var helper = new RepositoryQuery<T>(this);
            return helper;
        }

        internal async Task<IEnumerable<T>> Get(Expression<Func<T, bool>> filter = null,
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
        
            return await query.ToListAsync();
        }

        internal async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null)
        {
            IQueryable<T> query = _dbSet;

            includes?.ForEach(i => query.Include(i));
            
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.FirstOrDefaultAsync(filter);
        }
    }
}