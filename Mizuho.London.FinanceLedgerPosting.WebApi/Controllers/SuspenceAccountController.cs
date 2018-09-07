using System.Collections.Generic;
using System.Web.Http;
using Mizuho.London.Common.RoleProviders;

namespace Mizuho.London.FinanceLedgerPosting.WebApi.Controllers
{
    /// <summary>
    /// All about Suspence Accounts
    /// </summary>
    public class SuspenceAccountController : ApiController
    {
        /// <summary>
        /// Gets all the suspense accounts
        /// </summary>
        /// <returns></returns>
        // GET: api/SuspenceAccount
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { $"User: {User.Identity.Name}", $"Authenticated: {User.Identity.IsAuthenticated}", $"Authentication Type: {User.Identity.AuthenticationType}" };
        }

        // GET: api/SuspenceAccount/5
        [AllowAnonymous]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/SuspenceAccount
        /// <summary>
        /// Post suspense account data
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        [MizuhoAuthorize(Roles=Common.Constants.UserRoles.NotReadOnly)]
        public void CreateSuspenseAccount([FromBody]string value)
        {
        }

        // PUT: api/SuspenceAccount/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/SuspenceAccount/5
        public void Delete(int id)
        {
        }
    }
}
