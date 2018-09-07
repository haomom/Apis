namespace Mizuho.London.FinanceLedgerPosting.Interfaces.Model
{
    public interface ISuspenseAccount
    {
        int Id { get; set; }
        string BranchNo { get; set; }
        string Currency { get; set; }
        string AccountCode { get; set; }
        string AccountNoPart1 { get; set; }
        string AccountNoPart2 { get; set; }
    }
}
