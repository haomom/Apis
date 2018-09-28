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
                    UserCredentialId = 1,
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "1",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username1"
                },
                new UserCredential()
                {
                    UserCredentialId = 2,
                    Branch = "Milan",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "2",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username2"
                },
                new UserCredential()
                {
                    UserCredentialId = 3,
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "3",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username3"
                },
                new UserCredential()
                {
                    UserCredentialId = 4,
                    Branch = "Milan",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "4",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username4"
                },
                new UserCredential()
                {
                    UserCredentialId = 5,
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "5",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username5"
                },
                new UserCredential()
                {
                    UserCredentialId = 6,
                    Branch = "Milan",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "6",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username6"
                },
                new UserCredential()
                {
                    UserCredentialId = 7,
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "7",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username7"
                },
                new UserCredential()
                {
                    UserCredentialId = 8,
                    Branch = "Milan",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "8",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username8"
                },new UserCredential()
                {
                    UserCredentialId = 9,
                    Branch = "London",
                    ExpiryDate = DateTime.Today.Date,
                    GBaseEmployeeId = "9",
                    GBaseUserId = "xdf",
                    GBasePassword = "password",
                    UserName = "username9"
                },
                new UserCredential()
                {
                    UserCredentialId = 10,
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
                UserCredentialId = 1,
                Branch = "London",
                ExpiryDate = DateTime.Today.Date,
                GBaseEmployeeId = "1",
                GBaseUserId = "xdf",
                GBasePassword = "password",
                UserName = "username1"
            };
        }

        public static UserCredential GetInvalidUserCredential()
        {
            return new UserCredential()
            {
                UserCredentialId = 1,
                Branch = "London",
                ExpiryDate = DateTime.Today.Date,
                GBaseEmployeeId = "1",
                GBaseUserId = "",
                GBasePassword = "",
                UserName = "username1"
            };
        }

        public static UserCredential GetNewUserCredential()
        {
            return new UserCredential()
            {
                Branch = "London",
                ExpiryDate = DateTime.Today.Date,
                GBaseEmployeeId = "1",
                GBaseUserId = "sds",
                GBasePassword = "dsdsd",
                UserName = "newusername"
            };
        }
    }
}
