using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Mizuho.London.Common.RoleProviders;
using Mizuho.London.FinanceLedgerPosting.ModelDTO;

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
        public UserController(IUserCredentialService userCredentialService) : base()
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

            UserCredentialDTO returnObject = Mapper.Map<UserCredential, UserCredentialDTO>((UserCredential)userCredential);

            return Content(HttpStatusCode.Found, returnObject);
        }

        /// <summary>
        /// This create GBase user credential in Redis
        /// </summary>
        /// <param name="userCredential">User credential DTO model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UserCredentials/UserCredential/Create")]
        [MizuhoAuthorizeWebApi(Roles = Common.Constants.UserRoles.AnyRoleExceptReadOnly)]
        public async Task<IHttpActionResult> CreateUserCredential([FromBody] UserCredentialDTO userCredential)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Input not in correct format");
            }

            try
            {
                userCredential.UserName = User.Identity.Name;

                string resultMessage = await _userCredentialService.CreateUserCredential(userCredential);

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

        /// <summary>
        /// This removes GBase credential for current user for the given branch
        /// </summary>
        /// <param name="branch">branch</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/UserCredentials/UserCredential/Remove/{branch}")]
        [MizuhoAuthorizeWebApi(Roles = Common.Constants.UserRoles.AnyRoleExceptReadOnly)]
        public async Task<IHttpActionResult> RemoveUserCredential(string branch)
        {
            if (string.IsNullOrEmpty(branch))
            {
                return Content(HttpStatusCode.BadRequest, "Branch parameter is missing.");
            }

            try
            {
                string resultMessage = await _userCredentialService.RemoveUserCredential(User.Identity.Name, branch);

                if (!string.IsNullOrEmpty(resultMessage))
                {
                    return Content(HttpStatusCode.BadRequest, resultMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Ok("GBase credential removed successfully");
        }

        /// <summary>
        /// This updates GBase credential for current user for given branch
        /// </summary>
        /// <param name="userCredential">User credential DTO model</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/UserCredentials/UserCredential/Update")]
        [MizuhoAuthorizeWebApi(Roles = Common.Constants.UserRoles.AnyRoleExceptReadOnly)]
        public async Task<IHttpActionResult> UpdateUserCredential([FromBody] UserCredentialDTO userCredential)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Input not in correct format");
            }

            try
            {
                var resultMessage = await _userCredentialService.UpdateUserCredential(userCredential);

                if (!string.IsNullOrEmpty(resultMessage))
                {
                    return Content(HttpStatusCode.BadRequest, resultMessage);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Ok("GBase credential is updated successfully");
        }

        /// <summary>
        /// This triggers test of stored GBase credential in redis for the user
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="branch">Branch</param>
        /// <returns></returns>
        [Route("api/UserCredentials/UserCredential/Test/{userName}/{branch}")]
        public async Task<IHttpActionResult> TestGBaseCredential(string userName, string branch)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(branch))
            {
                return Content(HttpStatusCode.BadRequest, "One or more input parameters are missing.");
            }

            var resultMessage = await _userCredentialService.TestGBaseCredential(userName, branch);

            if (!string.IsNullOrEmpty(resultMessage))
            {
                return Content(HttpStatusCode.BadRequest, resultMessage);
            }

            return Ok();
        }
    }
}
