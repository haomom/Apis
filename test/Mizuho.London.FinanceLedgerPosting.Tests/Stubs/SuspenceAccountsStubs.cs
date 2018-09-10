using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Stubs
{
    public static class SuspenceAccountsStubs
    {
        public static List<SuspenseAccount> GetSuspenseAccounts()
        {
            List<SuspenseAccount> suspenceAccounts = new List<SuspenseAccount>
            {
                new SuspenseAccount
                {
                    SuspenseAccountId = 1,
                    Branch = "LDN",
                    Currency = "GBP",
                    AccountCode = "1234",
                    AccountNoPart1 = "xxx",
                    AccountNoPart2 = "yyy"
                },
                new SuspenseAccount
                {
                    SuspenseAccountId = 2,
                    Branch = "LDN",
                    Currency = "EUR",
                    AccountCode = "2568",
                    AccountNoPart1 = "xxx",
                    AccountNoPart2 = "yyy"
                },
                new SuspenseAccount
                {
                    SuspenseAccountId = 3,
                    Branch = "Milan",
                    Currency = "USD",
                    AccountCode = "1234",
                    AccountNoPart1 = "xxx",
                    AccountNoPart2 = "yyy"
                },
                new SuspenseAccount
                {
                    SuspenseAccountId = 3,
                    Branch = "Milan",
                    Currency = "USD",
                    AccountCode = "1234",
                    AccountNoPart1 = "xxx",
                    AccountNoPart2 = "yyy"
                }
            };

            return suspenceAccounts;
        }
    }
}
