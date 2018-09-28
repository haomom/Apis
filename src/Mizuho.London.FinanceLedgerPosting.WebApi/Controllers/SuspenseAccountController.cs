using Mizuho.London.Common.Logging;
using Mizuho.London.FinanceLedgerPosting.Common.Extensions;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Mizuho.London.Common.RoleProviders;
using Mizuho.London.FinanceLedgerPosting.ModelDTO;

namespace Mizuho.London.FinanceLedgerPosting.WebApi.Controllers
{
    /// <summary>
    /// All about Suspense Accounts
    /// </summary>
    [Authorize]
    public class SuspenseAccountController : BaseApiController
    {
        private readonly ISuspenseAccountRepository _suspenseAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMizLog _logger;

        /// <summary>
        /// This is the contructor of the Suspense account object
        /// </summary>
        /// <param name="suspenseAccountRepository">Suspense Account repository</param>
        /// <param name="unitOfWork">Unit of Work</param>
        /// <param name="logger">Miz Log instance</param>
        /// <param name="modelHelper">Model Helper instance</param>
        public SuspenseAccountController(ISuspenseAccountRepository suspenseAccountRepository, IUnitOfWork unitOfWork, IMizLog logger) : base()
        {
            _suspenseAccountRepository = suspenseAccountRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// This returns paged list of suspense accounts
        /// </summary>
        /// <param name="branch">branch to search for</param>
        /// <param name="currency">Currency to search for</param>
        /// <param name="pageNumber">page number. by default it is set to 1</param>
        /// <param name="pageSize">page size. by default it is set to 20</param>
        /// <returns>a paged list of suspense account objects</returns>
        [Route("api/SuspenseAccounts")]
        public async Task<IHttpActionResult> GetPagedList(string branch = null, string currency = null, int pageNumber = 1,
            int pageSize = 20)
        {
            var query = _suspenseAccountRepository.Query();

            Expression<Func<SuspenseAccount, bool>> filterExpressionTree = null;

            if (!string.IsNullOrEmpty(branch))
            {
                filterExpressionTree = filterExpressionTree.And(x => x.Branch.Contains(branch));
            }

            if (!string.IsNullOrEmpty(currency))
            {
                filterExpressionTree = filterExpressionTree.And(x => x.Currency == currency);
            }

            if (filterExpressionTree != null)
            {
                query.Filter(filterExpressionTree);
            }

            query.OrderBy(x => x.OrderBy(xx => xx.Branch).ThenBy(xx => xx.Currency));

            var result = await query.GetPage(pageNumber, pageSize);
            return Ok(result);
        }

        /// <summary>
        /// Gets suspense account by id
        /// </summary>
        /// <param name="id">Suspense account Id</param>
        /// <returns>Suspense Account Object</returns>
        [Route("api/SuspenseAccounts/SuspenseAccount/{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            SuspenseAccount suspenseAccount = await _suspenseAccountRepository.GetById(id);

            if (suspenseAccount == null)
            {
                return Content(HttpStatusCode.NotFound, "Suspense Account not found");
            }

            SuspenseAccountDTO suspenseAccountDto = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(suspenseAccount);
            return Ok(suspenseAccountDto);
        }

        /// <summary>
        /// This creates a new record of Suspense Account
        /// </summary>
        /// <param name="suspenseAccountModel">suspense Account DTO model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SuspenseAccounts/SuspenseAccount/Create")]
        [MizuhoAuthorizeWebApi(Roles = Common.Constants.UserRoles.AnyRoleExceptReadOnly)]
        public async Task<IHttpActionResult> CreateSuspenseAccount([FromBody] SuspenseAccountDTO suspenseAccountModel)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Invalid input");
            }

            SuspenseAccount newSuspenseAccount = null;
            try
            {
                SuspenseAccount existingModel = await _suspenseAccountRepository
                    .Query()
                    .Filter(x => x.Branch == suspenseAccountModel.Branch && x.Currency == suspenseAccountModel.Currency)
                    .GetFirstOrDefault();

                if (existingModel != null)
                {
                    return Content(HttpStatusCode.BadRequest,
                        "A suspense account already present for the given branch and currency");
                }

                newSuspenseAccount = new SuspenseAccount();

                newSuspenseAccount = Mapper.Map<SuspenseAccountDTO, SuspenseAccount>(suspenseAccountModel);

                newSuspenseAccount = _suspenseAccountRepository.Add(newSuspenseAccount);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                throw;
            }

            return Created(string.Empty, newSuspenseAccount);
        }

        /// <summary>
        /// This removes a suspense account record
        /// </summary>
        /// <param name="id">Suspense account id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SuspenseAccounts/SuspenseAccount/Remove/{id}")]
        [MizuhoAuthorizeWebApi(Roles = Common.Constants.UserRoles.AnyRoleExceptReadOnly)]
        public async Task<IHttpActionResult> RemoveSuspenseAccount(int id)
        {
            try
            {
                var existingRecord = await _suspenseAccountRepository.GetById(id);

                if (existingRecord == null)
                {
                    return Content(HttpStatusCode.NotFound, $"Suspense account record with id {id} does not exists");
                }

                _suspenseAccountRepository.Remove(existingRecord);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                throw;
            }

            return Ok($"Suspense account with id {id} is deleted");
        }

        /// <summary>
        /// This updates an existing suspense account details
        /// </summary>
        /// <param name="jsonBody">suspense account details to update</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/SuspenseAccounts/SuspenseAccount/Update")]
        [MizuhoAuthorizeWebApi(Roles = Common.Constants.UserRoles.AnyRoleExceptReadOnly)]
        public async Task<IHttpActionResult> UpdateSuspenseAccount([FromBody] SuspenseAccountDTO suspenseAccountModel)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Invalid input");
            }

            string errorMessage = await ValidateSuspenseAccountModel(suspenseAccountModel);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return Content(HttpStatusCode.BadRequest, errorMessage);
            }

            try
            {
                var modelFromBase = await _suspenseAccountRepository.GetById(suspenseAccountModel.SuspenseAccountId);

                 Mapper.Map<SuspenseAccountDTO, SuspenseAccount>(suspenseAccountModel, modelFromBase);

                _suspenseAccountRepository.Update(modelFromBase);
                _suspenseAccountRepository.SetEntityStateModified(modelFromBase);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                throw;
            }

            return Ok($"Suspense account with id {suspenseAccountModel.SuspenseAccountId} is updated successfully");
        }

        private async Task<string> ValidateSuspenseAccountModel(SuspenseAccountDTO suspenseAccountModel)
        {
            if (suspenseAccountModel.SuspenseAccountId == 0
                || string.IsNullOrEmpty(suspenseAccountModel.Branch)
                || string.IsNullOrEmpty(suspenseAccountModel.Currency)
                || string.IsNullOrEmpty(suspenseAccountModel.AccountCode)
                || string.IsNullOrEmpty(suspenseAccountModel.AccountNoPart1)
                || string.IsNullOrEmpty(suspenseAccountModel.AccountNoPart2))
            {
                return "Invalid Suspense Account Model. One of more required fields are missing.";
            }

            var existingModel = await _suspenseAccountRepository.GetById(suspenseAccountModel.SuspenseAccountId);

            if (existingModel == null)
            {
                return $"Suspense account with id {suspenseAccountModel.SuspenseAccountId} does not exists";
            }

            var duplicateModel = await
                _suspenseAccountRepository.FindSuspenseAccountByBranchCurrency(suspenseAccountModel.Branch,
                    suspenseAccountModel.Currency);

            if (duplicateModel != null &&
                duplicateModel.SuspenseAccountId != suspenseAccountModel.SuspenseAccountId)
            {
                return "There is another suspense account record with same branch and currency";
            }

            return string.Empty;
        }
    }
}
