using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;

namespace Mizuho.London.FinanceLedgerPosting.Services
{
    public class UserCredentialsSource : IUserCredentialsSource
    {
        public async Task<IList<UserCredential>> GetUserCredentials()
        {
            throw new NotImplementedException();
        }
    }

    public interface IUserCredentialsSource
    {
        Task<IList<UserCredential>> GetUserCredentials();
    }
}
