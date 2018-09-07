using Mizuho.London.FinanceLedgerPosting.Interfaces.Model;

namespace Mizuho.London.FinanceLedgerPosting.Common.Data
{
    public class SuspenseAccount : ISuspenseAccount
    {
        public int Id { get; set; }
        public string BranchNo { get; set; }
        public string Currency { get; set; }
        public string AccountCode { get; set; }
        public string AccountNoPart1 { get; set; }
        public string AccountNoPart2 { get; set; }

    }
}
