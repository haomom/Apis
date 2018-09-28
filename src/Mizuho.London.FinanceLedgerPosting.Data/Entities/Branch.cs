using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.Data.Entities
{
    public class Branch : IBranch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string BranchAccountCode { get; set; }
    }
}
