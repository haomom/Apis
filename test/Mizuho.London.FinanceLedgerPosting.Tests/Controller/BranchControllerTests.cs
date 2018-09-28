using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using AutoMapper;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.ModelDTO;
using Mizuho.London.FinanceLedgerPosting.Services;
using Mizuho.London.FinanceLedgerPosting.Tests.Helper;
using Mizuho.London.FinanceLedgerPosting.Tests.Stubs;
using Mizuho.London.FinanceLedgerPosting.WebApi.Controllers;
using Moq;
using NUnit.Framework;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Controller
{
    [TestFixture]
    public class BranchControllerTests
    {
        private Mock<IBranchService> _branchServiceMock;
        private const string ServiceBaseUrl = "http://localhost:59630";

        private readonly List<Branch> _source = BranchesStubs.GetAllBranches().ToList();
        private IBranch _existingBranch;

        [SetUp]
        public void Setup()
        {
            _branchServiceMock = new Mock<IBranchService>();
            _existingBranch = BranchesStubs.GetExistingBranch();

            Mapper.Initialize(cfg => { cfg.CreateMap<Branch, BranchDTO>(); });
        }

        private BranchController BranchController(string endpoint, HttpMethod requestMethod)
        {
            var api = new BranchController(_branchServiceMock.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage
                {
                    Method = requestMethod,
                    RequestUri = new Uri($"{ServiceBaseUrl}/{endpoint}")
                }
            };

            api.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            return api;
        }

        [Test]
        public async void When_calling_GetAllBranches_it_should_call_GetAllBranches_from_BranchService()
        {
            _branchServiceMock.Setup(x => x.GetAllBranches()).ReturnsAsync(_source);

            var api = BranchController("api/Branches", HttpMethod.Get);

            await api.GetAllBranches();

            _branchServiceMock.Verify(x => x.GetAllBranches());
        }

        [Test]
        public async void When_calling_GetAllBranches_it_should_return_all_entities()
        {
            _branchServiceMock.Setup(x => x.GetAllBranches()).ReturnsAsync(_source);

            var expectedObjects = _source.Select(x => Mapper.Map<Branch, BranchDTO>(x)).ToList();

            var api = BranchController("api/Branches", HttpMethod.Get);

            var response = await api.GetAllBranches();

            var result = response as OkNegotiatedContentResult<IEnumerable<BranchDTO>>;

            Assert.IsNotNull(result);
            ObjectComparer.ListOfPropertiesValuesAreEquals(result.Content.ToList(), expectedObjects);
        }

        [Test]
        [ExpectedException]
        public async void
            When_calling_GetAllBranches_it_should_throw_an_exception_when_BranchService_threw_an_exception()
        {
            _branchServiceMock.Setup(x => x.GetAllBranches()).Throws(new Exception());

            var api = BranchController("api/Branches", HttpMethod.Get);

            var response = await api.GetAllBranches();
        }

        [Test]
        public async void
            When_calling_GetBranchByBranchCode_it_should_return_httpstatus_BadRequest_if_branchcode_parameter_is_not_given()
        {
            string branchCode = string.Empty;

            var api = BranchController($"api/Branches/Branch/{branchCode}", HttpMethod.Get);

            var response = await api.GetBranchByBranchCode(branchCode);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("Branch code input parameter is empty", result.Content);
        }

        [Test]
        public async void When_callingGetBranchByBranchCode_it_should_call_GetByBranchCode_from_BranchService()
        {
            string branchCode = "xxx";

            _branchServiceMock.Setup(x => x.GetByBranchCode(branchCode)).ReturnsAsync(It.IsAny<IBranch>());

            var api = BranchController($"api/Branches/Branch/{branchCode}", HttpMethod.Get);

            var response = await api.GetBranchByBranchCode(branchCode);

            _branchServiceMock.Verify(x => x.GetByBranchCode(branchCode));
        }

        [Test]
        public async void When_callingGetBranchByBranchCode_it_should_return_httpstatus_NotFound_when_no_record_is_found()
        {
            string branchCode = "xxx";

            _branchServiceMock.Setup(x => x.GetByBranchCode(branchCode)).ReturnsAsync(It.IsAny<IBranch>());

            var api = BranchController($"api/Branches/Branch/{branchCode}", HttpMethod.Get);

            var response = await api.GetBranchByBranchCode(branchCode);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual($"There is no branch with branchcode '{branchCode}'", result.Content);
        }

        [Test]
        public async void When_callingGetBranchByBranchCode_it_should_return_httpstatus_Found_when_record_is_found()
        {
            string branchCode = "LDN";

            var expectedObject = Mapper.Map<Branch, BranchDTO>((Branch) _existingBranch);
            _branchServiceMock.Setup(x => x.GetByBranchCode(branchCode)).ReturnsAsync(_existingBranch);

            var api = BranchController($"api/Branches/Branch/{branchCode}", HttpMethod.Get);

            var response = await api.GetBranchByBranchCode(branchCode);

            var result = response as NegotiatedContentResult<BranchDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Found, result.StatusCode);
            ObjectComparer.PropertyValuesAreEquals(result.Content, expectedObject);
        }
    }
}
