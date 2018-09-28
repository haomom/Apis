using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;

namespace Mizuho.London.FinanceLedgerPosting.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<IEnumerable<IBranch>> GetAllBranches()
        {
            return await _branchRepository.GetAllEntities();
        }

        public async Task<IBranch> GetByBranchCode(string branchCode)
        {
            return await _branchRepository.Query().Filter(x => x.BranchCode == branchCode).GetFirstOrDefault();
        }
    }

    public interface IBranchService
    {
        Task<IEnumerable<IBranch>> GetAllBranches();

        Task<IBranch> GetByBranchCode(string branchCode);
    }
}
