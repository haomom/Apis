using System.Linq;

namespace Mizuho.London.FinanceLedgerPosting.Interfaces.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Delete(T model);
        void Add(T entity);
        void Update(T entity);
        T GetById(object id);
        IQueryable<T> GetAll();
        IRepositoryQuery<T> Query();
    }
}
