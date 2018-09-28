using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Repositories
{
    public class UserCredentialRepository : BaseRepository<UserCredential>, IUserCredentialRepository
    {
        public UserCredentialRepository(IUnitOfWork context) : base(context)
        {
        }
    }

    public interface IUserCredentialRepository : IRepository<UserCredential>
    {
    }
}
