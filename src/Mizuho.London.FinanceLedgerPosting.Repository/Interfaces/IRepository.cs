using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Remove(T model);
        T Add(T entity);
        void Update(T entity);
        void SetEntityStateModified(T entity);
        Task<T> GetById(object id);
        IQueryable<T> GetAll();
        IRepositoryQuery<T> Query();
        Task<IEnumerable<T>> GetAllEntities();
    }
}
