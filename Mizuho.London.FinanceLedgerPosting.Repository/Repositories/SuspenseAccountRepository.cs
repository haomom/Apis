using Mizuho.London.FinanceLedgerPosting.Common.Data;
using Mizuho.London.FinanceLedgerPosting.Interfaces.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Interfaces.Model;
using Mizuho.London.FinanceLedgerPosting.Interfaces.Repository;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using System.Linq;

namespace Mizuho.London.FinanceLedgerPosting.Repository
{
    public class SuspenseAccountRepository : BaseRepository<ISuspenseAccount>, ISuspenseAccountRepository
    {
        public SuspenseAccountRepository(IUnitOfWork context) : base(context)
        {
        }

        public ISuspenseAccount FindSuspenseAccountByBranchCurrency(string branchNo, string currency)
        {
            return GetAll().FirstOrDefault(sa => sa.BranchNo == branchNo && sa.Currency == currency);
        }
    }
}
