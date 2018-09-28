using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.Data.Entities
{
    public interface IBranch
    {
        int BranchId { get; set; }
        string BranchName { get; set; }
        string BranchCode { get; set; }
        string BranchAccountCode { get; set; }
    }
}
