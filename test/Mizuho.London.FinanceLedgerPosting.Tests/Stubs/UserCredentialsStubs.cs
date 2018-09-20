using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Stubs
{
    public static class UserCredentialsStubs
    {
        public static IList<UserCredential> GetAllUserCredentials()
        {
            return new List<UserCredential>()
            {
                new UserCredential()
                {
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "1",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username1"
                },
                new UserCredential()
                {
                    Branch = "Milan",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "2",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username2"
                },
                new UserCredential()
                {
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "3",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username3"
                },
                new UserCredential()
                {
                    Branch = "Milan",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "4",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username4"
                },
                new UserCredential()
                {
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "5",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username5"
                },
                new UserCredential()
                {
                    Branch = "Milan",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "6",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username6"
                },
                new UserCredential()
                {
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "7",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username7"
                },
                new UserCredential()
                {
                    Branch = "Milan",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "8",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username8"
                },new UserCredential()
                {
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "9",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username9"
                },
                new UserCredential()
                {
                    Branch = "Milan",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "10",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username10"
                }
            };
        }

        public static UserCredential GetExistingUserCredential()
        {
            return new UserCredential()
            {
                Branch = "London",
                ExpiryDate = DateTime.Today.Date,
                GBaseEmployeeId = "1",
                GBaseUserId = "xdf",
                GBasePassword = "password",
                UserName = "username1"
            };
        }
    }
}
