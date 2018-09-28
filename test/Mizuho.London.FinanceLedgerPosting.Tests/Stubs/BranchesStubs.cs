using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Stubs
{
    public static class BranchesStubs
    {
        public static IEnumerable<Branch> GetAllBranches()
        {
            return new List<Branch>
            {
                new Branch
                {
                    BranchId = 1,
                    BranchName = "London",
                    BranchCode = "LDN",
                    BranchAccountCode = "171"
                },
                new Branch
                {
                    BranchId = 2,
                    BranchName = "Milan",
                    BranchCode = "MIL",
                    BranchAccountCode = "170"
                },
                new Branch
                {
                    BranchId = 3,
                    BranchName = "Paris",
                    BranchCode = "PAR",
                    BranchAccountCode = "172"
                },
                new Branch
                {
                    BranchId = 2,
                    BranchName = "Amsterdam",
                    BranchCode = "AMS",
                    BranchAccountCode = "173"
                }
            };
        }

        public static IBranch GetExistingBranch()
        {
            return new Branch
            {
                BranchId = 1,
                BranchName = "London",
                BranchCode = "LDN",
                BranchAccountCode = "171"
            };
        }
    }
}
