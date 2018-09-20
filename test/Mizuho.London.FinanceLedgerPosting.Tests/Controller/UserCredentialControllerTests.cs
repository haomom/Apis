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
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Services;
using Mizuho.London.FinanceLedgerPosting.Tests.Helper;
using Mizuho.London.FinanceLedgerPosting.Tests.Stubs;
using Mizuho.London.FinanceLedgerPosting.WebApi.Controllers;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Controller
{
    [TestFixture]
    public class UserCredentialControllerTests
    {
        private Mock<IUserCredentialService> _mockUserCredentialServices;
        private const string ServiceBaseUrl = "http://localhost:59630";

        private IUserCredential _existingUserCredential;

        [SetUp]
        public void Setup()
        {
            _mockUserCredentialServices = new Mock<IUserCredentialService>();

            _existingUserCredential = UserCredentialsStubs.GetExistingUserCredential();
        }

        private UserController UserController(string endpoint, HttpMethod requestMethod)
        {
            var api = new UserController(_mockUserCredentialServices.Object)
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
        public async void When_calling_GetPagedList_it_should_return_HttpStatusCode_OK_when_it_is_successful()
        {
            _mockUserCredentialServices.Setup(x =>
                x.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult((IPagedResult<UserCredential>)null));

            var api = UserController("api/UserCredentials", HttpMethod.Get);

            var response = await api.GetPagedList(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            var result = response as OkNegotiatedContentResult<IPagedResult<UserCredential>>;

            Assert.IsNotNull(result);
        }

        [Test]
        public async void When_calling_GetPagedList_UserCredentialServices_GetPageUserCredentials_method_should_get_called()
        {
            _mockUserCredentialServices.Setup(x =>
                x.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult((IPagedResult<UserCredential>)null));

            var api = UserController("api/UserCredentials", HttpMethod.Get);

            var response = await api.GetPagedList(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>());

            _mockUserCredentialServices.Verify(x => x.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()));
        }

        [Test]
        public async void When_calling_Get_UserCredentialServices_GetUserCredentialForaBranch_method_should_get_called()
        {
            _mockUserCredentialServices.Setup(x =>
                    x.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult((IUserCredential)null));

            var api = UserController("api/UserCredentials/UserCredential/xxx", HttpMethod.Get);

            var response = await api.Get(It.IsAny<string>());

            _mockUserCredentialServices.Verify(x => x.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public async void When_calling_Get_UserCredentialServices_it_should_return_httpstatus_NotFound_when_there_is_no_record_for_the_given_criteria()
        {
            string branch = "xxx";

            _mockUserCredentialServices.Setup(x =>
                    x.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult((IUserCredential)null));

            var api = UserController($"api/UserCredentials/UserCredential/{branch}", HttpMethod.Get);

            var response = await api.Get(branch);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual($"There is no GBase credential for current user for {branch} branch.", result.Content);
        }

        [Test]
        public async void When_calling_Get_UserCredentialServices_it_should_return_httpstatus_Found_when_there_is_record_for_the_given_criteria()
        {
            string branch = _existingUserCredential.Branch;

            _mockUserCredentialServices.Setup(x =>
                    x.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_existingUserCredential));

            var api = UserController($"api/UserCredentials/UserCredential/{branch}", HttpMethod.Get);

            var response = await api.Get(branch);

            var result = response as NegotiatedContentResult<IUserCredential>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Found, result.StatusCode);
            ObjectComparer.PropertyValuesAreEquals(result.Content, _existingUserCredential);
        }

        [Test]
        public async void When_calling_CreateUserCredential_UserCredentialServices_CreateUserCredential_method_should_get_called()
        {
            _mockUserCredentialServices
                .Setup(x => x.CreateUserCredential(It.IsAny<JToken>()))
                .Returns(Task.FromResult(string.Empty));

            var api = UserController($"api/UserCredentials/CreateUserCredential", HttpMethod.Post);

            JToken body = JToken.FromObject(new UserCredential());

            var response = await api.CreateUserCredential(body);

            _mockUserCredentialServices.Verify(x => x.CreateUserCredential(It.IsAny<JToken>()));
        }

        [Test]
        public async void When_calling_CreateUserCredential_it_should_return_httpstatus_BadRequest_when_CreateUserCredential_from_UserCredentialServices_return_non_empty_string()
        {
            string errorMessage = "There an in issue with the request";

            _mockUserCredentialServices
                .Setup(x => x.CreateUserCredential(It.IsAny<JToken>()))
                .Returns(Task.FromResult(errorMessage));

            var api = UserController($"api/UserCredentials/CreateUserCredential", HttpMethod.Post);

            JToken body = JToken.FromObject(new UserCredential());

            var response = await api.CreateUserCredential(body);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(errorMessage, result.Content);
        }

        [Test]
        public async void When_calling_CreateUserCredential_it_should_return_httpstatus_Created_when_CreateUserCredential_from_UserCredentialServices_return_an_empty_string()
        {
            _mockUserCredentialServices
                .Setup(x => x.CreateUserCredential(It.IsAny<JToken>()))
                .Returns(Task.FromResult(string.Empty));

            var api = UserController($"api/UserCredentials/CreateUserCredential", HttpMethod.Post);

            JToken body = JToken.FromObject(new UserCredential());

            var response = await api.CreateUserCredential(body);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("GBase credential stored successfully", result.Content);
        }
    }
}
