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

        public ISuspenseAccount FindSuspenseAccountByBranchCurrency(string branch, string currency)
        {
            return Query().Filter(x => x.Branch == branch && x.Currency == currency).GetFirstOrDefault();
        }
    }

    public interface ISuspenseAccountRepository : IRepository<SuspenseAccount>
    {
        ISuspenseAccount FindSuspenseAccountByBranchCurrency(string branchNo, string currency);
    }
}
