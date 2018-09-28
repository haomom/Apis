namespace Mizuho.London.FinanceLedgerPosting.Data.Entities
{
    public interface ISuspenseAccount
    {
        int SuspenseAccountId { get; set; }
        string Branch { get; set; }
        string Currency { get; set; }
        string AccountCode { get; set; }
        string AccountNoPart1 { get; set; }
        string AccountNoPart2 { get; set; }
    }
}
