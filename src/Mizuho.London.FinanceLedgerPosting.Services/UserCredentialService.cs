using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Mizuho.London.Common.Logging;
using Mizuho.London.FinanceLedgerPosting.Common.Extensions;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mizuho.London.FinanceLedgerPosting.Services
{
    public class UserCredentialService : IUserCredentialService
    {
        private readonly IUserCredentialsSource _userCredentialsSource;
        private readonly IMizLog _logger;

        public UserCredentialService(IUserCredentialsSource userCredentialsSource, IMizLog logger)
        {
            _userCredentialsSource = userCredentialsSource;
            _logger = logger;
        }

        //TODO: Add sorting
        public async Task<IPagedResult<UserCredential>> GetPageUserCredentials(string branch, string userName, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                return new PagedResult<UserCredential>(Enumerable.Empty<UserCredential>(), 0, pageNumber, pageSize);

            IList<UserCredential> source = await _userCredentialsSource.GetUserCredentials();

            if (source == null || source.Count < 1)
                return new PagedResult<UserCredential>(Enumerable.Empty<UserCredential>(), 0, pageNumber, pageSize);

            var result = GetPagedList(branch, userName, pageNumber, pageSize, source);

            return new PagedResult<UserCredential>(result.Item1, result.Item2, pageNumber, pageSize);
        }

        public async Task<IUserCredential> GetUserCredentialForaBranch(string userName, string branch)
        {
            IList<UserCredential> source = await _userCredentialsSource.GetUserCredentials();

            return source?.FirstOrDefault(x => x.UserName == userName && x.Branch == branch);
        }

        public Task<string> CreateUserCredential(JToken newUserCredential)
        {
            throw new NotImplementedException();
        }

        private Tuple<IEnumerable<UserCredential>, int> GetPagedList(string branch, string userName, int pageNumber, int pageSize, IList<UserCredential> source)
        {
            IQueryable<UserCredential> query = source.AsQueryable();

            Expression<Func<UserCredential, bool>> filterExpressionTree = null;

            if (!string.IsNullOrEmpty(branch))
            {
                filterExpressionTree = filterExpressionTree.And(x => x.Branch.Contains(branch));
            }

            if (!string.IsNullOrEmpty(userName))
            {
                filterExpressionTree = filterExpressionTree.And(x => x.UserName == userName);
            }

            if (filterExpressionTree != null)
            {
                query = query.Where(filterExpressionTree);
            }

            /*if (orderBy != null)
            {
                query = orderBy(query);
            }*/

            var total = query.Count();

            if (pageNumber > 0 && pageSize > 0)
            {
                query = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize);
            }

            IEnumerable<UserCredential> list = query.ToList();
            return new Tuple<IEnumerable<UserCredential>, int>(list, total);
        }

        private async Task<Tuple<UserCredential, string>> ValidateuserCredentialModel(JToken jsonBody)
        {
            UserCredential userCredentialModel = null;
            string message = string.Empty;

            try
            {
                userCredentialModel = JsonConvert.DeserializeObject<UserCredential>(jsonBody.ToString());
            }
            catch (Exception e)
            {
                _logger.Error($"Invalid user credential model. Model: {jsonBody.ToString()} ", e);
                message = "Invalid UserCredential model";
                return Tuple.Create(userCredentialModel, message);
            }

            bool isValid = true;

            if (userCredentialModel == null || string.IsNullOrEmpty(userCredentialModel.Branch)
                                        || string.IsNullOrEmpty(userCredentialModel.GBaseEmployeeId)
                                        || string.IsNullOrEmpty(userCredentialModel.GBaseUserId)
                                        || string.IsNullOrEmpty(userCredentialModel.GBasePassword))
            {
                message = "Invalid UserCredential Model. One of more required fields are missing.";
                _logger.Error($"Invalid user credential model. Model: {jsonBody.ToString()} ");
                isValid = false;
            }

            /*if (isValid)
            {
                IList<UserCredential> source = await _userCredentialsSource.GetUserCredentials();

                var existingModel = source.FirstOrDefault(Ide);

                if (existingModel == null)
                {
                    message = $"Suspense account with id {userCredentialModel.SuspenseAccountId} does not exists";
                    isValid = false;
                }
                else
                {
                    var duplicateModel = await
                        _suspenseAccountRepository.FindSuspenseAccountByBranchCurrency(userCredentialModel.Branch,
                            suspenseAccountModel.Currency);

                    if (duplicateModel != null &&
                        duplicateModel.SuspenseAccountId != userCredentialModel.SuspenseAccountId)
                    {
                        message = "There is another suspense account record with same branch and currency";
                        isValid = false;
                    }
                }
            }*/

            if (!isValid)
                userCredentialModel = null;

            return Tuple.Create(userCredentialModel, message);
        }
    }

    public interface IUserCredentialService
    {
        Task<IPagedResult<UserCredential>> GetPageUserCredentials(string branch, string userName, int pageNumber, int pageSize);

        Task<IUserCredential> GetUserCredentialForaBranch(string userName, string branch);

        Task<string> CreateUserCredential(JToken newUserCredential);
    }
}
