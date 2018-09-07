namespace Mizuho.London.FinanceLedgerPosting.Data.Entities
{
    public class SuspenseAccount : ISuspenseAccount
    {
        public int SuspenseAccountId { get; set; }
        public string Branch { get; set; }
        public string Currency { get; set; }
        public string AccountCode { get; set; }
        public string AccountNoPart1 { get; set; }
        public string AccountNoPart2 { get; set; }
    }
}
