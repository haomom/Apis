using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using Mizuho.London.FinanceLedgerPosting.WebApi.Controllers;
using Moq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using NUnit.Framework;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Tests.Helper;
using Mizuho.London.FinanceLedgerPosting.Tests.Stubs;
using Newtonsoft.Json;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Controller
{
    [TestFixture]
    public class SuspenseAccountControllerTests
    {
        private Mock<ISuspenseAccountRepository> _mockSuspenseAccountRepository;
        private Mock<IRepositoryQuery<SuspenseAccount>> _mockSuspenceAccountRepositoryQuery;
        private const string ServiceBaseUrl = "http://localhost:59630";
        private SuspenseAccount _suspenceAccount;
        private SuspenseAccountRepository _suspenseAccountRepository;

        [SetUp]
        public void Setup()
        {
            
            var dbContext = new Mock<FinanceLedgerPostingDbContext>();
            dbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(() =>
                MockDBHelper.GetQueryableMockDbSet(SuspenceAccountsStubs.GetSuspenseAccounts()));

            _suspenseAccountRepository = new SuspenseAccountRepository(dbContext.Object);
            
            _mockSuspenseAccountRepository = new Mock<ISuspenseAccountRepository>();
            _mockSuspenceAccountRepositoryQuery = new Mock<IRepositoryQuery<SuspenseAccount>>();

            _suspenceAccount = new SuspenseAccount
            {
                SuspenseAccountId = 1,
                AccountCode = "1234",
                AccountNoPart1 = "567",
                AccountNoPart2 = "890",
                Currency = "GBP",
                Branch = "London"
            };
        }

        private SuspenceAccountController SuspenceAccountController(string endpoint, HttpMethod requestMethod, SuspenseAccountRepository suspenseAccountRepository)
        {
            var api = new SuspenceAccountController(suspenseAccountRepository ?? _mockSuspenseAccountRepository.Object)
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
        public void When_calling_Get_method_GetById_of_SuspenseAccountRepository_should_get_called()
        {
            _mockSuspenseAccountRepository.Setup(x => x.GetById(It.IsAny<int>()));

            var api = SuspenceAccountController("api/SuspenceAccount/1", HttpMethod.Get, null);

            var response = api.Get(It.IsAny<int>());

            _mockSuspenseAccountRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void When_calling_Get_method_it_should_return_a_suspenseaccount_object_if_it_is_present()
        {
            _mockSuspenseAccountRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(_suspenceAccount);

            var api = SuspenceAccountController("api/SuspenceAccount/1", HttpMethod.Get, null);

            var response = api.Get(It.IsAny<int>());

            var responseResult = JsonConvert.DeserializeObject<SuspenseAccount>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.IsNotNull(responseResult);
            ObjectComparer.PropertyValuesAreEquals(responseResult, _suspenceAccount);
        }

        [Test]
        public void When_calling_Get_method_it_should_return_a_objectnotfound_message_if_suspenseaccount_is_not_present()
        {
            _suspenceAccount = null;
            _mockSuspenseAccountRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(_suspenceAccount);

            var api = SuspenceAccountController("api/SuspenceAccount/1", HttpMethod.Get, null);

            var response = api.Get(It.IsAny<int>());

            var definition = new {Message = ""};
            var errorMessage = JsonConvert.DeserializeAnonymousType(response.Content.ReadAsStringAsync().Result, definition);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
            Assert.AreEqual(errorMessage.Message, "Suspense Account not found");
        }

        [Test]
        public void When_Calling_GetPagedList_Query_method_of_SuspenseAccountRepository_should_get_called()
        {
            _mockSuspenseAccountRepository.Setup(x => x.Query()).Returns(_mockSuspenceAccountRepositoryQuery.Object);
            
            var apiController = SuspenceAccountController("api/SuspenceAccount", HttpMethod.Get, null);

            var response = apiController.GetPagedList(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>());

            _mockSuspenseAccountRepository.Verify(x => x.Query(), Times.Once);
        }

        [Test]
        public void When_Calling_GetPagedList_with_Filter_by_branch_it_should_only_return_records_of_give_branch()
        {
            var apiController = SuspenceAccountController("api/SuspenceAccount", HttpMethod.Get, _suspenseAccountRepository);

            var response = apiController.GetPagedList("LDN", It.IsAny<string>(), 1, 10);

            var responseResult = JsonConvert.DeserializeObject<PagedResult<SuspenseAccount>>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseResult.RowCount, 2);
            ObjectComparer.AssertFilteredListByProperty(new SuspenseAccount(), responseResult.PagedList, "Branch", "LDN");
        }

        [Test]
        public void When_Calling_GetPagedList_with_Filter_by_currency_it_should_only_return_records_of_give_currency()
        {
            var apiController = SuspenceAccountController("api/SuspenceAccount", HttpMethod.Get, _suspenseAccountRepository);

            var response = apiController.GetPagedList(string.Empty, "USD", 1, 10);

            var responseResult = JsonConvert.DeserializeObject<PagedResult<SuspenseAccount>>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseResult.RowCount, 2);
            ObjectComparer.AssertFilteredListByProperty(new SuspenseAccount(), responseResult.PagedList, "Currency", "USD");
        }
    }
}
