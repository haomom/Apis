using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using Mizuho.London.FinanceLedgerPosting.WebApi.Controllers;
using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;
using AutoMapper;
using FluentAssertions;
using NUnit.Framework;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Tests.Helper;
using Mizuho.London.FinanceLedgerPosting.Tests.Stubs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Mizuho.London.Common.Logging;
using Mizuho.London.FinanceLedgerPosting.ModelDTO;
using Mizuho.London.FinanceLedgerPosting.WebApi.Common;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Controller
{
    [TestFixture]
    public class SuspenseAccountControllerTests
    {
        private Mock<IMizLog> _mockMizLog;
        private Mock<FinanceLedgerPostingDbContext> _mockDbContext;
        private Mock<ISuspenseAccountRepository> _mockSuspenseAccountRepository;
        private Mock<IRepositoryQuery<SuspenseAccount>> _mockSuspenceAccountRepositoryQuery;
        
        private SuspenseAccount _suspenseAccount;
        private SuspenseAccount _existingSuspenseAccount;
        private SuspenseAccount _invalidSuspenseAccount;
        private SuspenseAccountRepository _suspenseAccountRepository;
        private Mock<DbSet<SuspenseAccount>> _mockDbSet;

        private List<SuspenseAccount> _sourceList;
        private const string ServiceBaseUrl = "http://localhost:59630";
        private int _dbSetInitialCount;

        [SetUp]
        public void Setup()
        {
            _sourceList = SuspenceAccountsStubs.GetSuspenseAccounts();
            _mockMizLog = new Mock<IMizLog>();
            _mockDbContext = new Mock<FinanceLedgerPostingDbContext>();
            _mockSuspenseAccountRepository = new Mock<ISuspenseAccountRepository>();
            _mockSuspenceAccountRepositoryQuery = new Mock<IRepositoryQuery<SuspenseAccount>>();

            _mockDbSet = MockDBHelper.GetQueryableMockDbSet<SuspenseAccount>(_sourceList, (o => o.SuspenseAccountId));
            _mockDbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(() => _mockDbSet.Object);
            _suspenseAccountRepository = new SuspenseAccountRepository(_mockDbContext.Object);
            
            _suspenseAccount = SuspenceAccountsStubs.GetNewSuspenseAccount();
            _existingSuspenseAccount = SuspenceAccountsStubs.GetExistingSuspenseAccount();
            _invalidSuspenseAccount = SuspenceAccountsStubs.GetInvalidSuspenseAccount();

            _dbSetInitialCount = _mockDbSet.Object.Count();

            Mapper.Initialize(cfg => {
                cfg.CreateMap<SuspenseAccountDTO, SuspenseAccount>();
                cfg.CreateMap<SuspenseAccount, SuspenseAccountDTO>();
            });
        }

        private SuspenseAccountController SuspenceAccountController(string endpoint, HttpMethod requestMethod, SuspenseAccountRepository suspenseAccountRepository)
        {
            var api = new SuspenseAccountController(suspenseAccountRepository ?? _mockSuspenseAccountRepository.Object, _mockDbContext.Object, _mockMizLog.Object)
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
        public async void When_calling_Get_method_GetById_of_SuspenseAccountRepository_should_get_calledAsync()
        {
            _mockSuspenseAccountRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult((SuspenseAccount)null));

            var api = SuspenceAccountController("api/SuspenseAccounts/SuspenseAccount/1", HttpMethod.Get, null);

            await api.Get(It.IsAny<int>());

            _mockSuspenseAccountRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once());
        }

        [Test]
        public async Task When_calling_Get_method_it_should_return_a_suspenseaccount_object_if_it_is_present()
        {
            _mockSuspenseAccountRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(_suspenseAccount));

            var api = SuspenceAccountController("api/SuspenseAccounts/SuspenseAccount/1", HttpMethod.Get, null);

            IHttpActionResult response = await api.Get(It.IsAny<int>());

            var result = response as OkNegotiatedContentResult<SuspenseAccountDTO>;

            SuspenseAccountDTO expectedObject = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_suspenseAccount);

            Assert.IsNotNull(result);
            ObjectComparer.PropertyValuesAreEquals(result.Content, expectedObject);
        }

        [Test]
        public async Task When_calling_Get_method_it_should_return_a_objectnotfound_message_if_suspenseaccount_is_not_present()
        {
            _suspenseAccount = null;
            _mockSuspenseAccountRepository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(_suspenseAccount));

            var api = SuspenceAccountController("api/SuspenseAccounts/SuspenseAccount/1", HttpMethod.Get, null);

            IHttpActionResult response = await api.Get(It.IsAny<int>());

            var result = response as NegotiatedContentResult<string>;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("Suspense Account not found", result.Content);
        }

        [Test]
        public async Task When_Calling_GetPagedList_Query_method_of_SuspenseAccountRepository_should_get_called()
        {
            _mockSuspenseAccountRepository.Setup(x => x.Query()).Returns(_mockSuspenceAccountRepositoryQuery.Object);
            
            var apiController = SuspenceAccountController("api/SuspenseAccounts", HttpMethod.Get, null);

            var response = await apiController.GetPagedList(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(),
                It.IsAny<int>());

            _mockSuspenseAccountRepository.Verify(x => x.Query(), Times.Once);
        }

        [Test]
        public async Task When_Calling_GetPagedList_with_Filter_by_branch_it_should_only_return_records_of_give_branch()
        {
            var apiController = SuspenceAccountController("api/SuspenseAccounts", HttpMethod.Get, _suspenseAccountRepository);

            var response = await apiController.GetPagedList("LDN", It.IsAny<string>(), 1, 10);

            var result = response as OkNegotiatedContentResult<IPagedResult<SuspenseAccount>>;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Content.RowCount);
            ObjectComparer.AssertFilteredListByProperty(new SuspenseAccount(), result.Content.PagedList, "Branch", "LDN");
        }

        [Test]
        public async Task When_Calling_GetPagedList_with_Filter_by_currency_it_should_only_return_records_of_give_currency()
        {
            var apiController = SuspenceAccountController("api/SuspenseAccounts", HttpMethod.Get, _suspenseAccountRepository);

            var response = await apiController.GetPagedList(string.Empty, "USD", 1, 10);

            var result = response as OkNegotiatedContentResult<IPagedResult<SuspenseAccount>>;

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Content.RowCount);
            ObjectComparer.AssertFilteredListByProperty(new SuspenseAccount(), result.Content.PagedList, "Currency", "USD");
        }

        [Test]
        public async void When_Calling_CreateSuspenseAccount_it_should_call_Add_method_of_SuspenseAccountRepository()
        {
            _mockSuspenseAccountRepository.Setup(d => d.Query()).Returns(_mockSuspenceAccountRepositoryQuery.Object);
            _mockSuspenceAccountRepositoryQuery.Setup(x => x.Filter(It.IsAny<Expression<Func<SuspenseAccount, bool>>>())).Returns(_mockSuspenceAccountRepositoryQuery.Object);
            _mockSuspenceAccountRepositoryQuery.Setup(x => x.GetFirstOrDefault()).Returns(Task.FromResult((SuspenseAccount)null));

            var apiController = SuspenceAccountController("api/CreateSuspenseAccount", HttpMethod.Post, null);

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_suspenseAccount);

            await apiController.CreateSuspenseAccount(body);
            
            _mockSuspenseAccountRepository.Verify(x => x.Add(It.IsAny<SuspenseAccount>()), Times.Once);
        }

        [Test]
        public async Task When_Calling_CreateSuspenseAccount_it_should_create_a_new_record()
        {
            var apiController = SuspenceAccountController("api/CreateSuspenseAccount", HttpMethod.Post, _suspenseAccountRepository);

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_suspenseAccount);

            var response = await apiController.CreateSuspenseAccount(body);

            var result = response as CreatedNegotiatedContentResult<SuspenseAccount>;

            Assert.IsNotNull(result);
            Assert.AreEqual(_dbSetInitialCount + 1, _mockDbSet.Object.Count());
        }

        [Test]
        public async Task When_Calling_CreateSuspenseAccount_it_should_return_an_error_when_same_branch_currency_record_exists()
        {
            var apiController = SuspenceAccountController("api/CreateSuspenseAccount", HttpMethod.Post, _suspenseAccountRepository);

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_existingSuspenseAccount);

            var response = await apiController.CreateSuspenseAccount(body);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("A suspense account already present for the given branch and currency", result.Content);
        }

        [Test]
        public async Task When_Calling_RemoveSuspenseAccount_it_should_return_HttpStatus_Ok_when_it_is_successful()
        {
            int id = 1;

            var apiController = SuspenceAccountController($"api/RemoveSuspenseAccount/{id}", HttpMethod.Post, _suspenseAccountRepository);

            var response = await apiController.RemoveSuspenseAccount(id);

            var result = response as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual($"Suspense account with id {id} is deleted", result.Content);
        }

        [Test]
        public async Task When_Calling_RemoveSuspenseAccount_it_should_return_HttpStatus_NotFound_when_suspense_account_doesn_not_exists()
        {
            int id = 1;

            var apiController = SuspenceAccountController($"api/RemoveSuspenseAccount/{id}", HttpMethod.Post, null);

            var response = await apiController.RemoveSuspenseAccount(id);
            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual($"Suspense account record with id {id} does not exists", result.Content);
        }

        [Test]
        public async Task When_Calling_RemoveSuspenseAccount_it_should_call_Remove_method_of_Respository()
        {
            int id = 1;

            _mockSuspenseAccountRepository.Setup(x => x.GetById(id)).Returns(Task.FromResult(SuspenceAccountsStubs.GetExistingSuspenseAccount()));

            var apiController = SuspenceAccountController($"api/RemoveSuspenseAccount/{id}", HttpMethod.Post, null);

            var response = await apiController.RemoveSuspenseAccount(id);

            _mockSuspenseAccountRepository.Verify(x => x.Remove(It.IsAny<SuspenseAccount>()));
        }

        [Test]
        public async Task When_Calling_RemoveSuspenseAccount_it_should_remove_the_record()
        {
            int id = 1;
            
            var apiController = SuspenceAccountController($"api/RemoveSuspenseAccount/{id}", HttpMethod.Post, _suspenseAccountRepository);

            var response = await apiController.RemoveSuspenseAccount(id);
            var result = response as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual($"Suspense account with id {id} is deleted", result.Content);
            Assert.AreEqual(_dbSetInitialCount - 1, _mockDbSet.Object.Count());
        }

        [Test]
        public async Task When_Calling_UpdateSuspenseAccount_it_should_return_HttpStatus_Ok_when_it_is_successful()
        {
            var apiController = SuspenceAccountController("api/UpdateSuspenseAccount", HttpMethod.Post, null);

            _mockSuspenseAccountRepository.Setup(x => x.SetEntityStateModified(It.IsAny<SuspenseAccount>()));
            _mockSuspenseAccountRepository.Setup(x => x.Update(It.IsAny<SuspenseAccount>()));
            _mockSuspenseAccountRepository.Setup(x => x.GetById(_existingSuspenseAccount.SuspenseAccountId)).Returns(Task.FromResult(_existingSuspenseAccount));
            _mockSuspenseAccountRepository.Setup(x => x.FindSuspenseAccountByBranchCurrency(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(_existingSuspenseAccount));

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_existingSuspenseAccount);

            var response = await apiController.UpdateSuspenseAccount(body);
            var result = response as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual($"Suspense account with id {_existingSuspenseAccount.SuspenseAccountId} is updated successfully", result.Content);
        }

        [Test]
        public async Task When_Calling_UpdateSuspenseAccount_it_should_return_HttpStatus_BadRequest_when_suspense_account_doesn_not_exists()
        {
            var apiController = SuspenceAccountController("api/UpdateSuspenseAccount", HttpMethod.Post, _suspenseAccountRepository);

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_suspenseAccount);

            var response = await apiController.UpdateSuspenseAccount(body);
            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual($"Suspense account with id {_suspenseAccount.SuspenseAccountId} does not exists", result.Content);
        }

        [Ignore("will come back to dealign with this")]
        public async Task When_Calling_UpdateSuspenseAccount_it_should_return_HttpStatus_BadRequest_when_suspense_account_model_is_not_valid()
        {
            var apiController = SuspenceAccountController("api/UpdateSuspenseAccount", HttpMethod.Post, null);

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_invalidSuspenseAccount);

            var response = await apiController.UpdateSuspenseAccount(body);
            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("This is error message", result.Content);
        }

        [Test]
        public async Task When_Calling_UpdateSuspenseAccount_it_should_return_HttpStatus_BadRequest_when_suspense_account_model_is_not_valid_2()
        {
            var apiController = SuspenceAccountController("api/UpdateSuspenseAccount", HttpMethod.Post, null);

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_invalidSuspenseAccount);

            var response = await apiController.UpdateSuspenseAccount(body);
            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("Invalid Suspense Account Model. One of more required fields are missing.", result.Content);
        }

        [Test]
        public async Task When_Calling_UpdateSuspenseAccount_it_should_return_HttpStatus_BadRequest_when_duplicate_branch_currency_suspense_account_exists()
        {
            var apiController = SuspenceAccountController("api/UpdateSuspenseAccount", HttpMethod.Post, _suspenseAccountRepository);

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(SuspenceAccountsStubs.GetExistingSuspenseAccountWithDuplicateValues());

            var response = await apiController.UpdateSuspenseAccount(body);
            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual($"There is another suspense account record with same branch and currency", result.Content);
        }

        [Test]
        public async Task When_Calling_UpdateSuspenseAccount_it_should_call_Update_method_of_SuspenseAccountRepository()
        {
            _mockSuspenseAccountRepository.Setup(x => x.Update(It.IsAny<SuspenseAccount>()));
            _mockSuspenseAccountRepository.Setup(x => x.GetById(_existingSuspenseAccount.SuspenseAccountId)).Returns(Task.FromResult(_existingSuspenseAccount));
            _mockSuspenseAccountRepository.Setup(x => x.FindSuspenseAccountByBranchCurrency(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(_existingSuspenseAccount));

            var apiController = SuspenceAccountController("api/UpdateSuspenseAccount", HttpMethod.Post, null);

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_existingSuspenseAccount);

            var response = await apiController.UpdateSuspenseAccount(body);

            _mockSuspenseAccountRepository.Verify(x => x.Update(It.IsAny<SuspenseAccount>()));
        }

        [Test]
        public async Task When_Calling_UpdateSuspenseAccount_it_should_call_SetEntityStateModified_method_of_SuspenseAccountRepository()
        {
            _mockSuspenseAccountRepository.Setup(x => x.SetEntityStateModified(It.IsAny<SuspenseAccount>()));
            _mockSuspenseAccountRepository.Setup(x => x.Update(It.IsAny<SuspenseAccount>()));
            _mockSuspenseAccountRepository.Setup(x => x.GetById(_existingSuspenseAccount.SuspenseAccountId)).Returns(Task.FromResult(_existingSuspenseAccount));
            _mockSuspenseAccountRepository.Setup(x => x.FindSuspenseAccountByBranchCurrency(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(_existingSuspenseAccount));

            var apiController = SuspenceAccountController("api/UpdateSuspenseAccount", HttpMethod.Post, null);

            var body = Mapper.Map<SuspenseAccount, SuspenseAccountDTO>(_existingSuspenseAccount);

            var response = await apiController.UpdateSuspenseAccount(body);

            _mockSuspenseAccountRepository.Verify(x => x.SetEntityStateModified(It.IsAny<SuspenseAccount>()));
        }
    }
}
