using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mizuho.London.Common.Logging;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Services;
using Mizuho.London.FinanceLedgerPosting.Tests.Helper;
using Mizuho.London.FinanceLedgerPosting.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Services
{
    [TestFixture]
    public class UserCredentialServicesTests
    {
        private Mock<IUserCredentialsSource> _userCredentialsSourceMock;
        private Mock<IMizLog> _mockMizLog;

        private IList<UserCredential> _sourceList;

        [SetUp]
        public void Setup()
        {
            _userCredentialsSourceMock = new Mock<IUserCredentialsSource>();
            _mockMizLog = new Mock<IMizLog>();

            _sourceList = UserCredentialsStubs.GetAllUserCredentials();
        }

        [Test]
        public async void When_calling_GetPageUserCredentials_it_should_return_empty_list_if_pagesize_is_less_than_1()
        {
            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);
            var response = await userCredentialService.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), 1, 0);

            Assert.IsEmpty(response.PagedList);
        }

        [Test]
        public async void When_calling_GetPageUserCredentials_it_should_return_empty_list_if_pagenumber_is_less_than_1()
        {
            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);
            var response = await userCredentialService.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), 0, 1);

            Assert.IsEmpty(response.PagedList);
        }

        [Test]
        public async void When_calling_GetPageUserCredentials_it_should_call_GetUserCredentials_of_UserCredentialsSource()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), 1, 1);

            _userCredentialsSourceMock.Verify(x => x.GetUserCredentials());
        }

        [Test]
        public async void When_calling_GetPageUserCredentials_it_should_return_empty_list_if_there_is_no_usercredentials()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult((IList<UserCredential>)null));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), 1, 1);

            Assert.IsEmpty(response.PagedList);
        }

        [Test]
        public async void When_calling_GetPageUserCredentials_it_should_return_first_elements_if_no_filter_parameters_are_provided()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetPageUserCredentials(null, null, 1, 5);

            var expectedElements = _sourceList.Where(x => Convert.ToInt32(x.GBaseEmployeeId) <= 5).ToList();

            Assert.AreEqual(5, response.PagedList.Count());
            ObjectComparer.ListOfPropertiesValuesAreEquals(response.PagedList.ToList(), expectedElements);
        }

        [Test]
        public async void When_calling_GetPageUserCredentials_it_should_return_paged_elements_if_pagenumber_is_provided()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetPageUserCredentials(null, null, 2, 2);

            var expectedElements = _sourceList.Where(x => Convert.ToInt32(x.GBaseEmployeeId) == 3 || Convert.ToInt32(x.GBaseEmployeeId) == 4).ToList();

            Assert.AreEqual(2, response.PagedList.Count());
            ObjectComparer.ListOfPropertiesValuesAreEquals(response.PagedList.ToList(), expectedElements);
        }

        [Test]
        public async void When_calling_GetPageUserCredentials_it_should_return_elements_of_filter_parameters()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetPageUserCredentials("London", null, 1, 10);

            ObjectComparer.AssertFilteredListByProperty(new UserCredential(), response.PagedList, "Branch", "London");
        }

        [Test]
        public async void When_calling_GetPageUserCredentials_it_should_return_elements_of_filter_parameters_1()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetPageUserCredentials(null, "username2", 1, 10);

            ObjectComparer.AssertFilteredListByProperty(new UserCredential(), response.PagedList, "UserName", "username2");
        }

        [Test]
        public async void
            When_calling_GetUserCredentialForaBranch_it_should_call_GetUserCredentials_of_UserCredentialsSource()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>());

            _userCredentialsSourceMock.Verify(x => x.GetUserCredentials());
        }

        [Test]
        public async void
            When_calling_GetUserCredentialForaBranch_it_should_return_null_when_source_is_null()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult((IList<UserCredential>)null));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>());

            Assert.AreEqual(null, response);
        }

        [Test]
        public async void
            When_calling_GetUserCredentialForaBranch_it_should_return_object_which_satisfy_given_criteria()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetUserCredentialForaBranch("username1", "London");

            var expectedElement = _sourceList.FirstOrDefault(x => x.UserName == "username1" && x.Branch == "London");

            Assert.IsNotNull(response);
            ObjectComparer.PropertyValuesAreEquals(response, expectedElement);
        }

        [Test]
        public async void
            When_calling_GetUserCredentialForaBranch_it_should_return_null_object_when_given_criteria_is_not_satisfied()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            UserCredentialService userCredentialService = new UserCredentialService(_userCredentialsSourceMock.Object, _mockMizLog.Object);

            var response = await userCredentialService.GetUserCredentialForaBranch("username1", "xxx");

            Assert.AreEqual(null, response);
        }
    }
}
