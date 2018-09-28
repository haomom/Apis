using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Repositories
{
    public class BranchRepository : BaseRepository<Branch>, IBranchRepository
    {
        public BranchRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }

    public interface IBranchRepository : IRepository<Branch>
    {
    }
}
