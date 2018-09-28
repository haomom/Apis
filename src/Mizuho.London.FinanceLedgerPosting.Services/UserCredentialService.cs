using Mizuho.London.Common.Logging;
using Mizuho.London.FinanceLedgerPosting.Common.Extensions;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Mizuho.London.FinanceLedgerPosting.ModelDTO;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using UserCredential = Mizuho.London.FinanceLedgerPosting.Data.Entities.UserCredential;

namespace Mizuho.London.FinanceLedgerPosting.Services
{
    public class UserCredentialService : IUserCredentialService
    {
        private readonly IUserCredentialRepository _userCredentialRepository;
        private readonly IMizLog _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UserCredentialService(IUserCredentialRepository userCredentialRepository, IMizLog logger, IUnitOfWork unitOfWork)
        {
            _userCredentialRepository = userCredentialRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        //TODO: sorting
        public async Task<IPagedResult<UserCredential>> GetPageUserCredentials(string branch, string userName, int pageNumber, int pageSize)
        {
            var query = _userCredentialRepository.Query();

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
                query.Filter(filterExpressionTree);
            }

            query.OrderBy(x => x.OrderBy(xx => xx.UserName).ThenBy(xx => xx.Branch));

            return await query.GetPage(pageNumber, pageSize);
        }

        public async Task<IUserCredential> GetUserCredentialForaBranch(string userName, string branch)
        {
            return await _userCredentialRepository
                                    .Query()
                                    .Filter(x => x.UserName == userName && x.Branch == branch)
                                    .GetFirstOrDefault();
        }

        public async Task<string> CreateUserCredential(UserCredentialDTO newUserCredential)
        {
            var validationResult = await ValidateUserCredentialModel(newUserCredential, "create");

            if (!string.IsNullOrEmpty(validationResult.Item2))
            {
                return validationResult.Item2;
            }

            var userCredential = Mapper.Map<UserCredentialDTO, UserCredential>(newUserCredential);
            userCredential.LastModifiedOn = DateTime.Now;
            userCredential.CreatedOn = DateTime.Now;
            userCredential.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;

            UserCredential newRecord = _userCredentialRepository.Add(userCredential);
            _unitOfWork.Commit();
            
            return (newRecord != null)
                ? string.Empty
                : $"Error while adding GBase credential for {newUserCredential.UserName} for {newUserCredential.Branch} branch";
        }

        public async Task<string> RemoveUserCredential(string userName, string branch)
        {
            var existingRecord = await _userCredentialRepository.Query().Filter(x => x.UserName == userName && x.Branch == branch).GetFirstOrDefault();
            
            if (existingRecord == null)
            {
                return $"GBase credential record with for user {userName} and {branch} branch does not exists";
            }

            _userCredentialRepository.Remove(existingRecord);
            _unitOfWork.Commit();

            return string.Empty;
        }

        public async Task<string> UpdateUserCredential(UserCredentialDTO updatedUserCredential)
        {
            var validateResult = await ValidateUserCredentialModel(updatedUserCredential, "update");

            if (!string.IsNullOrEmpty(validateResult.Item2))
            {
                return validateResult.Item2;
            }

            UserCredential existingUserCredential = validateResult.Item1;
            Mapper.Map<UserCredentialDTO, UserCredential>(updatedUserCredential, existingUserCredential);

            existingUserCredential.LastModifiedOn = DateTime.Now;
            existingUserCredential.ModifiedBy = Thread.CurrentPrincipal.Identity.Name;

            _userCredentialRepository.Update(existingUserCredential);
            _userCredentialRepository.SetEntityStateModified(existingUserCredential);
            _unitOfWork.Commit();

            return string.Empty;
        }

        public async Task<string> TestGBaseCredential(string userName, string branch)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(branch))
            {
                return $"User name and/or branch parameters are missing.";
            }

            UserCredential existingRecord = await _userCredentialRepository.Query().Filter(x => x.UserName == userName && x.Branch == branch).GetFirstOrDefault();

            if (existingRecord == null)
            {
                return $"GBase credential record with for user {userName} and {branch} branch does not exists";
            }

            return string.Empty;
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

        private async Task<Tuple<UserCredential, string>> ValidateUserCredentialModel(
            UserCredentialDTO userCredentialModel, string action)
        {
            if (userCredentialModel == null)
            {
                return new Tuple<UserCredential, string>(null, "Invalid UserCredential Model.");
            }

            if (!(!string.IsNullOrEmpty(userCredentialModel.Branch)
                  && !string.IsNullOrEmpty(userCredentialModel.GBaseEmployeeId)
                  && !string.IsNullOrEmpty(userCredentialModel.GBaseUserId)
                  && !string.IsNullOrEmpty(userCredentialModel.GBasePassword)
                  && !string.IsNullOrEmpty(userCredentialModel.UserName)
                  && userCredentialModel.ExpiryDate != DateTime.MinValue))
            {
                return new Tuple<UserCredential, string>(null,
                    "Invalid UserCredential Model. One of more required fields are missing.");
            }

            if (string.IsNullOrEmpty(action))
            {
                return new Tuple<UserCredential, string>(null, string.Empty);
            }

            var existingRecord = await _userCredentialRepository.Query().Filter(x =>
                    x.UserName == userCredentialModel.UserName && x.Branch == userCredentialModel.Branch)
                .GetFirstOrDefault();

            if (action == "create")
            {
                if (existingRecord != null)
                {
                    return new Tuple<UserCredential, string>(null,
                        "GBase user credential record already exists for the current user and branch");
                }
            }
            else if (action == "update")
            {
                if (existingRecord == null)
                {
                    return new Tuple<UserCredential, string>(null,
                        "GBase user credential record does not exists for the current user and branch");
                }
                else
                {
                    return new Tuple<UserCredential, string>(existingRecord, string.Empty);
                }
            }
            
            return new Tuple<UserCredential, string>(null, string.Empty);
        }
    }

    public interface IUserCredentialService
    {
        Task<IPagedResult<UserCredential>> GetPageUserCredentials(string branch, string userName, int pageNumber, int pageSize);

        Task<IUserCredential> GetUserCredentialForaBranch(string userName, string branch);

        Task<string> CreateUserCredential(UserCredentialDTO newUserCredential);

        Task<string> RemoveUserCredential(string userName, string branch);

        Task<string> UpdateUserCredential(UserCredentialDTO newUserCredential);

        Task<string> TestGBaseCredential(string userName, string branch);
    }
}
