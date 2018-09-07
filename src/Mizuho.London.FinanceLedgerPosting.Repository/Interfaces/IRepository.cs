using System.Linq;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Interfaces
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
