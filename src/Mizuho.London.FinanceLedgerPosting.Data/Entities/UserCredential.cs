using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.Data.Entities
{
    public class UserCredential : IUserCredential
    {
        public int UserCredentialId { get; set; }
        public string UserName { get; set; }
        public string Branch { get; set; }
        public string GBaseUserId { get; set; }
        public string GBaseEmployeeId { get; set; }
        public string GBasePassword { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastModifiedOn { get; set; }
    }
}
