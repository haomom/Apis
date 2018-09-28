using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.ModelDTO
{
    public class SuspenseAccountDTO
    {
        public int SuspenseAccountId { get; set; }
        public string Branch { get; set; }
        public string Currency { get; set; }
        public string AccountCode { get; set; }
        public string AccountNoPart1 { get; set; }
        public string AccountNoPart2 { get; set; }
    }
}
