using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.ModelDTO;
using Mizuho.London.FinanceLedgerPosting.Services;

namespace Mizuho.London.FinanceLedgerPosting.WebApi.Controllers
{
    /// <summary>
    /// All about branches
    /// </summary>
    public class BranchController : BaseApiController
    {
        private readonly IBranchService _branchService;

        /// <summary>
        /// Branch constructor
        /// </summary>
        public BranchController(IBranchService branchService) : base()
        {
            _branchService = branchService;
        }

        /// <summary>
        /// This returns all the branches
        /// </summary>
        /// <returns></returns>
        [Route("api/Branches")]
        public async Task<IHttpActionResult> GetAllBranches()
        {
            var branches = await _branchService.GetAllBranches();

            var branchDtoList = branches.Select(x => Mapper.Map<Branch, BranchDTO>((Branch) x));

            return Ok(branchDtoList);
        }

        /// <summary>
        /// This returns branch detail for the queried branch
        /// </summary>
        /// <returns></returns>
        [Route("api/Branches/Branch/{branchCode}")]
        public async Task<IHttpActionResult> GetBranchByBranchCode(string branchCode)
        {
            if (string.IsNullOrEmpty(branchCode))
            {
                return Content(HttpStatusCode.BadRequest, "Branch code input parameter is empty");
            }

            var branch = await _branchService.GetByBranchCode(branchCode);

            if (branch == null)
            {
                return Content(HttpStatusCode.NotFound, $"There is no branch with branchcode '{branchCode}'");
            }

            var branchDto = Mapper.Map<Branch, BranchDTO>((Branch)branch);

            return Content(HttpStatusCode.Found, branchDto);
        }
    }
}
