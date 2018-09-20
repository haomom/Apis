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
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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
        public SuspenseAccountController(ISuspenseAccountRepository suspenseAccountRepository, IUnitOfWork unitOfWork, IMizLog logger)
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
            else
            {
                return Ok(suspenseAccount);
            }
        }
        
        /// <summary>
        /// This creates a new record of Suspense Account
        /// </summary>
        /// <param name="jsonBody">suspense Account details</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SuspenseAccounts/CreateSuspenseAccount")]
        public async Task<IHttpActionResult> CreateSuspenseAccount([FromBody] JToken jsonBody)
        {
            SuspenseAccount newSuspenseAccount = null;
            try
            {
                SuspenseAccount suspenseAccountModel = JsonConvert.DeserializeObject<SuspenseAccount>(jsonBody.ToString());

                SuspenseAccount existingModel = await _suspenseAccountRepository
                    .Query()
                    .Filter(x => x.Branch == suspenseAccountModel.Branch && x.Currency == suspenseAccountModel.Currency)
                    .GetFirstOrDefault();

                if (existingModel == null)
                {
                    newSuspenseAccount = _suspenseAccountRepository.Add(suspenseAccountModel);
                    _unitOfWork.Commit();
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "A suspense account already present for the given branch and currency");
                }
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
        [Route("api/SuspenseAccounts/RemoveSuspenseAccount/{id}")]
        public async Task<IHttpActionResult> RemoveSuspenseAccount(int id)
        {
            try
            {
                var existingRecord = await _suspenseAccountRepository.GetById(id);

                if (existingRecord != null)
                {
                    _suspenseAccountRepository.Remove(existingRecord);
                    _unitOfWork.CommitAsync();
                }
                else
                {
                    return Content(HttpStatusCode.NotFound, $"Suspense account record with id {id} does not exists");
                }
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
        [Route("api/SuspenseAccounts/UpdateSuspenseAccount")]
        public async Task<IHttpActionResult> UpdateSuspenseAccount([FromBody] JToken jsonBody)
        {
            Tuple<SuspenseAccount, string> result = await ValidateSuspenseAccountModel(jsonBody);

            string errorMessage = result.Item2;
            SuspenseAccount suspenseAccountModel = result.Item1;

            if (suspenseAccountModel != null)
            {
                try
                {
                    _suspenseAccountRepository.Update(suspenseAccountModel);
                    _suspenseAccountRepository.SetEntityStateModified(suspenseAccountModel);
                    _unitOfWork.Commit();
                }
                catch (Exception)
                {
                    throw;
                }

                return Ok($"Suspense account with id {suspenseAccountModel.SuspenseAccountId} is updated successfully");
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, errorMessage);
            }
        }

        private async Task<Tuple<SuspenseAccount, string>> ValidateSuspenseAccountModel(JToken jsonBody)
        {
            SuspenseAccount suspenseAccountModel = null;
            string message = string.Empty;

            try
            {
                suspenseAccountModel = JsonConvert.DeserializeObject<SuspenseAccount>(jsonBody.ToString());
            }
            catch (Exception e)
            {
                _logger.Error($"Invalid suspense account model. Model: {jsonBody.ToString()} ", e);
                message = "Invalid Suspense account model";
                return Tuple.Create(suspenseAccountModel, message);
            }

            bool isValid = true;

            if (suspenseAccountModel == null || string.IsNullOrEmpty(suspenseAccountModel.Branch)
                                        || string.IsNullOrEmpty(suspenseAccountModel.Currency)
                                        || string.IsNullOrEmpty(suspenseAccountModel.AccountCode)
                                        || string.IsNullOrEmpty(suspenseAccountModel.AccountNoPart1)
                                        || string.IsNullOrEmpty(suspenseAccountModel.AccountNoPart2))
            {
                message = "Invalid Suspense Account Model. One of more required fields are missing.";
                _logger.Error($"Invalid suspense account model. Model: {jsonBody.ToString()} ");
                isValid = false;
            }

            if (isValid)
            {
                var existingModel = await _suspenseAccountRepository.GetById(suspenseAccountModel.SuspenseAccountId);

                if (existingModel == null)
                {
                    message = $"Suspense account with id {suspenseAccountModel.SuspenseAccountId} does not exists";
                    isValid = false;
                }
                else
                {
                    var duplicateModel = await
                        _suspenseAccountRepository.FindSuspenseAccountByBranchCurrency(suspenseAccountModel.Branch,
                            suspenseAccountModel.Currency);

                    if (duplicateModel != null &&
                        duplicateModel.SuspenseAccountId != suspenseAccountModel.SuspenseAccountId)
                    {
                        message = "There is another suspense account record with same branch and currency";
                        isValid = false;
                    }
                }
            }

            if (!isValid)
                suspenseAccountModel = null;

            return Tuple.Create(suspenseAccountModel, message);
        }
    }
}
