using System.Threading.Tasks;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Repositories
{
    public class SuspenseAccountRepository : BaseRepository<SuspenseAccount>, ISuspenseAccountRepository
    {
        public SuspenseAccountRepository(IUnitOfWork context) : base(context)
        {
        }

        public async Task<SuspenseAccount> FindSuspenseAccountByBranchCurrency(string branch, string currency)
        {
            var query = Query().Filter(x => x.Branch == branch && x.Currency == currency);
            return await query.GetFirstOrDefault();
        }
    }

    public interface ISuspenseAccountRepository : IRepository<SuspenseAccount>
    {
        Task<SuspenseAccount> FindSuspenseAccountByBranchCurrency(string branch, string currency);
    }
}
