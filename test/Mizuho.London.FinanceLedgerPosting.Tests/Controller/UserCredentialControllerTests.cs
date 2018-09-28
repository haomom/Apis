using AutoMapper;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.ModelDTO;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Services;
using Mizuho.London.FinanceLedgerPosting.Tests.Helper;
using Mizuho.London.FinanceLedgerPosting.Tests.Stubs;
using Mizuho.London.FinanceLedgerPosting.WebApi.Controllers;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Results;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Controller
{
    [TestFixture]
    public class UserCredentialControllerTests
    {
        private Mock<IUserCredentialService> _mockUserCredentialServices;
        private const string ServiceBaseUrl = "http://localhost:59630";

        private IUserCredential _existingUserCredential;
        private string _userName;
        private string _branch;

        [SetUp]
        public void Setup()
        {
            _mockUserCredentialServices = new Mock<IUserCredentialService>();

            _existingUserCredential = UserCredentialsStubs.GetExistingUserCredential();

            _userName = "userName";
            _branch = "branch";

            Mapper.Initialize(cfg => {
                cfg.CreateMap<UserCredentialDTO, UserCredential>();
                cfg.CreateMap<UserCredential, UserCredentialDTO>();
            });
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
        [Category("UserCredentialController > GetPagedList")]
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
        [Category("UserCredentialController > GetPagedList")]
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
        [Category("UserCredentialController > Get")]
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
        [Category("UserCredentialController > Get")]
        public async void When_calling_Get_UserCredentialServices_it_should_return_httpstatus_NotFound_when_there_is_no_record_for_the_given_criteria()
        {
            _mockUserCredentialServices.Setup(x =>
                    x.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult((IUserCredential)null));

            var api = UserController($"api/UserCredentials/UserCredential/{_branch}", HttpMethod.Get);

            var response = await api.Get(_branch);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual($"There is no GBase credential for current user for {_branch} branch.", result.Content);
        }

        [Test]
        [Category("UserCredentialController > Get")]
        public async void When_calling_Get_UserCredentialServices_it_should_return_httpstatus_Found_when_there_is_record_for_the_given_criteria()
        {
            string branch = _existingUserCredential.Branch;

            _mockUserCredentialServices.Setup(x =>
                    x.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(_existingUserCredential));

            var expectedObject = Mapper.Map<UserCredential, UserCredentialDTO>((UserCredential)_existingUserCredential);

            var api = UserController($"api/UserCredentials/UserCredential/{branch}", HttpMethod.Get);

            var response = await api.Get(branch);

            var result = response as NegotiatedContentResult<UserCredentialDTO>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Found, result.StatusCode);
            ObjectComparer.PropertyValuesAreEquals(result.Content, expectedObject);
        }
        
        [Test]
        [Category("UserCredentialController > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_it_should_return_HttpStatus_BadRequest_when_input_is_not_correct()
        {
            _mockUserCredentialServices
                .Setup(x => x.CreateUserCredential(It.IsAny<UserCredentialDTO>()))
                .ReturnsAsync("This is error message");

            var api = UserController($"api/UserCredentials/CreateUserCredential", HttpMethod.Post);

            var body = Mapper.Map<UserCredential, UserCredentialDTO>(UserCredentialsStubs.GetNewUserCredential());

            var response = await api.CreateUserCredential(body);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("This is error message", result.Content);
        }

        [Test]
        [Category("UserCredentialController > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_UserCredentialServices_CreateUserCredential_method_should_get_called()
        {
            _mockUserCredentialServices
                .Setup(x => x.CreateUserCredential(It.IsAny<UserCredentialDTO>()))
                .Returns(Task.FromResult(string.Empty));

            var api = UserController($"api/UserCredentials/CreateUserCredential", HttpMethod.Post);

            var body = Mapper.Map<UserCredential, UserCredentialDTO>(UserCredentialsStubs.GetNewUserCredential());

            var response = await api.CreateUserCredential(body);

            _mockUserCredentialServices.Verify(x => x.CreateUserCredential(It.IsAny<UserCredentialDTO>()));
        }

        [Test]
        [Category("UserCredentialController > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_it_should_return_httpstatus_BadRequest_when_CreateUserCredential_from_UserCredentialServices_return_non_empty_string()
        {
            string errorMessage = "There an in issue with the request";

            _mockUserCredentialServices
                .Setup(x => x.CreateUserCredential(It.IsAny<UserCredentialDTO>()))
                .Returns(Task.FromResult(errorMessage));

            var api = UserController($"api/UserCredentials/CreateUserCredential", HttpMethod.Post);

            var body = new UserCredentialDTO();

            var response = await api.CreateUserCredential(body);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(errorMessage, result.Content);
        }

        [Test]
        [Category("UserCredentialController > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_it_should_return_httpstatus_Created_when_CreateUserCredential_from_UserCredentialServices_return_an_empty_string()
        {
            _mockUserCredentialServices
                .Setup(x => x.CreateUserCredential(It.IsAny<UserCredentialDTO>()))
                .Returns(Task.FromResult(string.Empty));

            var api = UserController($"api/UserCredentials/CreateUserCredential", HttpMethod.Post);

            var body = new UserCredentialDTO();

            var response = await api.CreateUserCredential(body);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("GBase credential stored successfully", result.Content);
        }

        [Test]
        [Category("UserCredentialController > RemoveUserCredential")]
        public async void When_calling_RemoveUserCredential_it_should_call_RemoveUserCredential_from_UserCredentialServices()
        {
            string branch = "London";

            _mockUserCredentialServices
                .Setup(x => x.RemoveUserCredential(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<string>()));

            var api = UserController($"api/UserCredentials/RemoveUserCredential/{branch}", HttpMethod.Post);

            var response = await api.RemoveUserCredential(branch);

            _mockUserCredentialServices.Verify(x => x.RemoveUserCredential(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        [Category("UserCredentialController > RemoveUserCredential")]
        public async void When_calling_RemoveUserCredential_it_should_return_httpstatus_BadRequest_when_RemoveUserCredential_from_UserCredentialServices_returns_error_message()
        {
            string branch = "London";

            _mockUserCredentialServices
                .Setup(x => x.RemoveUserCredential(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult("This is an error message"));

            var api = UserController($"api/UserCredentials/RemoveUserCredential/{branch}", HttpMethod.Post);

            var response = await api.RemoveUserCredential(branch);
            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("This is an error message", result.Content);
        }

        [Test]
        [Category("UserCredentialController > RemoveUserCredential")]
        public async void When_calling_RemoveUserCredential_it_should_return_httpstatus_Ok_when_RemoveUserCredential_from_UserCredentialServices_returns_empty_message()
        {
            string branch = "London";

            _mockUserCredentialServices
                .Setup(x => x.RemoveUserCredential(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(It.IsAny<string>()));

            var api = UserController($"api/UserCredentials/RemoveUserCredential/{branch}", HttpMethod.Post);

            var response = await api.RemoveUserCredential(branch);
            var result = response as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual("GBase credential removed successfully", result.Content);
        }
        
        [Test]
        [Category("UserCredentialController > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_return_HttpStatus_BadRequest_when_input_is_not_correct()
        {
            _mockUserCredentialServices.Setup(x => x.UpdateUserCredential(It.IsAny<UserCredentialDTO>())).ReturnsAsync("This is error message");

            var api = UserController("api/UserCredentials/UserCredential/Update", HttpMethod.Put);

            var body = Mapper.Map<UserCredential, UserCredentialDTO>(UserCredentialsStubs.GetInvalidUserCredential());

            var response = await api.UpdateUserCredential(body);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual("This is error message", result.Content);
        }

        [Test]
        [Category("UserCredentialController > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_call_UpdateUserCredential_from_UserCredentialService()
        {
            _mockUserCredentialServices
                .Setup(x => x.UpdateUserCredential(It.IsAny<UserCredentialDTO>()))
                .Returns(Task.FromResult(string.Empty));

            var api = UserController("api/UserCredentials/UserCredential/Update", HttpMethod.Put);

            var body = new UserCredentialDTO();

            var response = await api.UpdateUserCredential(body);

            _mockUserCredentialServices.Verify(x => x.UpdateUserCredential(It.IsAny<UserCredentialDTO>()));
        }

        [Test]
        [Category("UserCredentialController > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_return_httpstatus_BadRequest_when_UpdateUserCredential_from_UserCredentialServices_return_non_empty_message()
        {
            string errorMessage = "There an in issue with the request";

            _mockUserCredentialServices
                .Setup(x => x.UpdateUserCredential(It.IsAny<UserCredentialDTO>()))
                .Returns(Task.FromResult(errorMessage));

            var api = UserController("api/UserCredentials/UserCredential/Update", HttpMethod.Put);

            var body = new UserCredentialDTO();

            var response = await api.UpdateUserCredential(body);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(errorMessage, result.Content);
        }

        [Test]
        [Category("UserCredentialController > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_return_httpstatus_Created_when_UpdateUserCredential_from_UserCredentialServices_return_an_empty_string()
        {
            _mockUserCredentialServices
                .Setup(x => x.UpdateUserCredential(It.IsAny<UserCredentialDTO>()))
                .Returns(Task.FromResult(string.Empty));

            var api = UserController("api/UserCredentials/UserCredential/Update", HttpMethod.Put);

            var body = new UserCredentialDTO();

            var response = await api.UpdateUserCredential(body);

            var result = response as OkNegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual("GBase credential is updated successfully", result.Content);
        }

        [Test]
        [Category("UserCredentialController > TestUserCredential")]
        public async void When_calling_TestGBaseCredential_it_should_httpstatus_BadRequest_if_one_or_more_input_parameters_are_missing()
        {
            _branch = string.Empty;

            _mockUserCredentialServices.Setup(x => x.TestGBaseCredential(_userName, _branch)).ReturnsAsync(It.IsAny<string>());

            var api = UserController("api/UserCredentials/UserCredential/Test/xxx", HttpMethod.Get);

            var response = await api.TestGBaseCredential(_userName, _branch);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual($"One or more input parameters are missing.", result.Content);
        }

        [Test]
        [Category("UserCredentialController > TestUserCredential")]
        public async void When_calling_TestGBaseCredential_it_should_call_TestGBaseCredential_from_UserCredentialService()
        {
            _mockUserCredentialServices.Setup(x => x.TestGBaseCredential(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<string>());

            var api = UserController("api/UserCredentials/UserCredential/Test/xxx", HttpMethod.Get);

            var response = await api.TestGBaseCredential(_userName, _branch);

            _mockUserCredentialServices.Verify(x => x.TestGBaseCredential(It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        [Category("UserCredentialController > TestUserCredential")]
        public async void When_calling_TestGBaseCredential_it_should_return_httpstatus_BadRequest_If_UserCredentialService_returns_non_empty_message()
        {
            _mockUserCredentialServices.Setup(x => x.TestGBaseCredential(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("This is error message.");

            var api = UserController("api/UserCredentials/UserCredential/Test/xxx", HttpMethod.Get);

            var response = await api.TestGBaseCredential(_userName, _branch);

            var result = response as NegotiatedContentResult<string>;

            Assert.IsNotNull(result);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual($"This is error message.", result.Content);
        }

        [Test]
        [Category("UserCredentialController > TestUserCredential")]
        public async void When_calling_TestGBaseCredential_it_should_return_httpstatus_ok_If_UserCredentialService_returns_empty_message()
        {
            _mockUserCredentialServices.Setup(x => x.TestGBaseCredential(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(string.Empty);

            var api = UserController("api/UserCredentials/UserCredential/Test/xxx", HttpMethod.Get);

            var response = await api.TestGBaseCredential(_userName, _branch);

            var result = response as OkResult;

            Assert.IsNotNull(result);
        }
    }
}
