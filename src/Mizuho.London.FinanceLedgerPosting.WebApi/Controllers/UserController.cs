using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Services;
using Newtonsoft.Json.Linq;

namespace Mizuho.London.FinanceLedgerPosting.WebApi.Controllers
{
    /// <summary>
    /// All about user
    /// </summary>
    public class UserController : BaseApiController
    {
        private readonly IUserCredentialService _userCredentialService;

        /// <summary>
        /// This is the contructor for UserController
        /// </summary>
        /// <param name="userCredentialService"></param>
        public UserController(IUserCredentialService userCredentialService)
        {
            _userCredentialService = userCredentialService;
        }

        /// <summary>
        /// This returns paged list of all GBase credentials stored in redis
        /// </summary>
        /// <param name="branch">Application Branch</param>
        /// <param name="userName">User name</param>
        /// <param name="pageNumber">current page numer</param>
        /// <param name="pageSize">page size</param>
        /// <returns>A paged list of all user credentials</returns>
        [Route("api/UserCredentials")]
        public async Task<IHttpActionResult> GetPagedList(string branch = null, string userName = null, int pageNumber = 1, int pageSize = 20)
        {
            IPagedResult<UserCredential> userCredentials = await _userCredentialService.GetPageUserCredentials(branch, userName, pageNumber, pageSize);

            return Ok(userCredentials);
        }

        /// <summary>
        /// This returns the current user's G Base credential stored in redis for a given branch
        /// </summary>
        /// <returns></returns>
        [Route("api/UserCredentials/UserCredential/{branch}")]
        public async Task<IHttpActionResult> Get(string branch)
        {
            IUserCredential userCredential =
                await _userCredentialService.GetUserCredentialForaBranch(User.Identity.Name, branch);

            if (userCredential == null)
            {
                return Content(HttpStatusCode.NotFound,
                    $"There is no GBase credential for current user for {branch} branch.");
            }

            return Content(HttpStatusCode.Found, userCredential);
        }

        /// <summary>
        /// This create GBase user credential in Redis
        /// </summary>
        /// <param name="jsonBody">User credential model in json</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UserCredentials/CreateUserCredential")]
        public async Task<IHttpActionResult> CreateUserCredential([FromBody] JToken jsonBody)
        {
            try
            {
                string resultMessage = await _userCredentialService.CreateUserCredential(jsonBody);

                if (!string.IsNullOrEmpty(resultMessage))
                {
                    return Content(HttpStatusCode.BadRequest, resultMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Content(HttpStatusCode.Created, "GBase credential stored successfully");
        }
    }
}
