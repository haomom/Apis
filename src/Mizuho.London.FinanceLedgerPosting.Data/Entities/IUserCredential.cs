using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.Data.Entities
{
    public interface IUserCredential
    {
        string UserName { get; set; }
        string Branch { get; set; }
        string GBaseUserId { get; set; }
        string GBaseEmployeeId { get; set; }
        string GBasePassword { get; set; }
        DateTime ExpiryDate { get; set; }
    }
}
