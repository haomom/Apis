using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.Data.Entities
{
    public class UserCredential : IUserCredential
    {
        public string UserName { get; set; }
        public string Branch { get; set; }
        public string GBaseUserId { get; set; }
        public string GBaseEmployeeId { get; set; }
        public string GBasePassword { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
