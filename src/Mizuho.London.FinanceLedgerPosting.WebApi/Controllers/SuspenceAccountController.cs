using Mizuho.London.Common.RoleProviders;
using Mizuho.London.FinanceLedgerPosting.Common.Extensions;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;

namespace Mizuho.London.FinanceLedgerPosting.WebApi.Controllers
{
    /// <summary>
    /// All about Suspence Accounts
    /// </summary>
    public class SuspenceAccountController : ApiController
    {
        private readonly ISuspenseAccountRepository _suspenseAccountRepository;

        /// <summary>
        /// This is the contructor of the Suspense account object
        /// </summary>
        /// <param name="suspenseAccountRepository"></param>
        public SuspenceAccountController(ISuspenseAccountRepository suspenseAccountRepository)
        {
            _suspenseAccountRepository = suspenseAccountRepository;
        }

        /// <summary>
        /// Gets suspense account by id
        /// </summary>
        /// <param name="id">Suspense account Id</param>
        /// <returns>Suspense Account Object</returns>
        // GET: api/SuspenceAccount/5
        public HttpResponseMessage Get(int id)
        {
            var suspenseAccount = _suspenseAccountRepository.GetById(id);

            return suspenseAccount != null
                ? Request.CreateResponse<SuspenseAccount>(HttpStatusCode.OK, suspenseAccount)
                : Request.CreateErrorResponse(HttpStatusCode.NotFound, "Suspense Account not found");
        }
        
        /// <summary>
        /// This returns paged list of suspense accounts
        /// </summary>
        /// <param name="branch">branch to search for</param>
        /// <param name="currency">Currency to search for</param>
        /// <param name="pageNumber">page number. by default it is set to 1</param>
        /// <param name="pageSize">page size. by default it is set to 20</param>
        /// <returns>a paged list of suspense account objects</returns>
        public HttpResponseMessage GetPagedList(string branch = null, string currency = null, int pageNumber = 1,
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

            return Request.CreateResponse(HttpStatusCode.OK, query.GetPage(pageNumber, pageSize));
        }
    }
}
