using Mizuho.London.FinanceLedgerPosting.Interfaces.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Interfaces.Model;

namespace Mizuho.London.FinanceLedgerPosting.Interfaces.Repository
{
    public interface ISuspenseAccountRepository : IRepository<ISuspenseAccount>
    {
       ISuspenseAccount FindSuspenseAccountByBranchCurrency(string branchNo, string currency);
    }
}
