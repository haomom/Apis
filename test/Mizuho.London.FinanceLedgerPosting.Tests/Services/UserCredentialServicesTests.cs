using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mizuho.London.Common.Logging;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
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
        //private Mock<IUserCredentialRepository> _mockUserCredentialRepository;
        private Mock<IMizLog> _mockMizLog;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<DbSet<UserCredential>> _mockUserCredentialDbSet;
        private Mock<FinanceLedgerPostingDbContext> _mockDbContext;

        private IUserCredentialRepository _userCredentialRepository;
        private UserCredentialService _userCredentialService;
        private IList<UserCredential> _sourceList;

        [SetUp]
        public void Setup()
        {
            //_mockUserCredentialRepository = new Mock<IUserCredentialRepository>();
            _mockMizLog = new Mock<IMizLog>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _sourceList = UserCredentialsStubs.GetAllUserCredentials();
            _mockUserCredentialDbSet = MockDBHelper.GetQueryableMockDbSet<UserCredential>(_sourceList.ToList(), (o => o.UserCredentialId));
            _mockDbContext = new Mock<FinanceLedgerPostingDbContext>();
            _mockDbContext.Setup(x => x.Set<UserCredential>()).Returns(_mockUserCredentialDbSet.Object);

            _userCredentialRepository = new UserCredentialRepository(_mockDbContext.Object);

            _userCredentialService = new UserCredentialService(_userCredentialRepository, _mockMizLog.Object, _mockUnitOfWork.Object);
        }

        [Test]
        [Category("UserCredentialService > GetPageUserCredentials")]
        public async void When_calling_GetPageUserCredentials_it_should_return_empty_list_if_pagesize_is_less_than_1()
        {
            var response = await _userCredentialService.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), 1, 0);

            Assert.IsEmpty(response.PagedList);
        }

        [Test]
        [Category("UserCredentialService > GetPageUserCredentials")]
        public async void When_calling_GetPageUserCredentials_it_should_return_empty_list_if_pagenumber_is_less_than_1()
        {
            var response = await _userCredentialService.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), 0, 1);

            Assert.IsEmpty(response.PagedList);
        }
        /*
        [Test]
        [Category("UserCredentialService > GetPageUserCredentials")]
        public async void When_calling_GetPageUserCredentials_it_should_call_GetUserCredentials_of_UserCredentialsSource()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));
            
            var response = await _userCredentialService.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), 1, 1);

            _userCredentialsSourceMock.Verify(x => x.GetUserCredentials());
        }

        [Test]
        [Category("UserCredentialService > GetPageUserCredentials")]
        public async void When_calling_GetPageUserCredentials_it_should_return_empty_list_if_there_is_no_usercredentials()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult((IList<UserCredential>)null));

            var response = await _userCredentialService.GetPageUserCredentials(It.IsAny<string>(), It.IsAny<string>(), 1, 1);

            Assert.IsEmpty(response.PagedList);
        }

        [Test]
        [Category("UserCredentialService > GetPageUserCredentials")]
        public async void When_calling_GetPageUserCredentials_it_should_return_first_elements_if_no_filter_parameters_are_provided()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            var response = await _userCredentialService.GetPageUserCredentials(null, null, 1, 5);

            var expectedElements = _sourceList.Where(x => Convert.ToInt32(x.GBaseEmployeeId) <= 5).ToList();

            Assert.AreEqual(5, response.PagedList.Count());
            ObjectComparer.ListOfPropertiesValuesAreEquals(response.PagedList.ToList(), expectedElements);
        }

        [Test]
        [Category("UserCredentialService > GetPageUserCredentials")]
        public async void When_calling_GetPageUserCredentials_it_should_return_paged_elements_if_pagenumber_is_provided()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            var response = await _userCredentialService.GetPageUserCredentials(null, null, 2, 2);

            var expectedElements = _sourceList.Where(x => Convert.ToInt32(x.GBaseEmployeeId) == 3 || Convert.ToInt32(x.GBaseEmployeeId) == 4).ToList();

            Assert.AreEqual(2, response.PagedList.Count());
            ObjectComparer.ListOfPropertiesValuesAreEquals(response.PagedList.ToList(), expectedElements);
        }

        [Test]
        [Category("UserCredentialService > GetPageUserCredentials")]
        public async void When_calling_GetPageUserCredentials_it_should_return_elements_of_filter_parameters()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            var response = await _userCredentialService.GetPageUserCredentials("London", null, 1, 10);

            ObjectComparer.AssertFilteredListByProperty(new UserCredential(), response.PagedList, "Branch", "London");
        }

        [Test]
        [Category("UserCredentialService > GetPageUserCredentials")]
        public async void When_calling_GetPageUserCredentials_it_should_return_elements_of_filter_parameters_1()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            var response = await _userCredentialService.GetPageUserCredentials(null, "username2", 1, 10);

            ObjectComparer.AssertFilteredListByProperty(new UserCredential(), response.PagedList, "UserName", "username2");
        }

        [Test]
        [Category("UserCredentialService > GetUserCredentialForaBranch")]
        public async void
            When_calling_GetUserCredentialForaBranch_it_should_call_GetUserCredentials_of_UserCredentialsSource()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            var response = await _userCredentialService.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>());

            _userCredentialsSourceMock.Verify(x => x.GetUserCredentials());
        }

        [Test]
        [Category("UserCredentialService > GetUserCredentialForaBranch")]
        public async void
            When_calling_GetUserCredentialForaBranch_it_should_return_null_when_source_is_null()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult((IList<UserCredential>)null));

            var response = await _userCredentialService.GetUserCredentialForaBranch(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsNull(response);
        }

        [Test]
        [Category("UserCredentialService > GetUserCredentialForaBranch")]
        public async void
            When_calling_GetUserCredentialForaBranch_it_should_return_object_which_satisfy_given_criteria()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            var response = await _userCredentialService.GetUserCredentialForaBranch("username1", "London");

            var expectedElement = _sourceList.FirstOrDefault(x => x.UserName == "username1" && x.Branch == "London");

            Assert.IsNotNull(response);
            ObjectComparer.PropertyValuesAreEquals(response, expectedElement);
        }

        [Test]
        [Category("UserCredentialService > GetUserCredentialForaBranch")]
        public async void
            When_calling_GetUserCredentialForaBranch_it_should_return_null_object_when_given_criteria_is_not_satisfied()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            var response = await _userCredentialService.GetUserCredentialForaBranch("username1", "xxx");

            Assert.AreEqual(null, response);
        }

        [Test]
        [Category("UserCredentialService > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_it_should_return_error_message_if_input_object_is_null()
        {
            var resultMessage = await _userCredentialService.CreateUserCredential(null);

            Assert.AreEqual("Invalid UserCredential Model.", resultMessage);
        }

        [Test]
        [Category("UserCredentialService > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_it_should_return_error_message_if_input_object_has_empty_properties()
        {
            var resultMessage = await _userCredentialService.CreateUserCredential(UserCredentialsStubs.GetInvalidUserCredential());

            Assert.AreEqual("Invalid UserCredential Model. One of more required fields are missing.", resultMessage);
        }

        [Test]
        [Category("UserCredentialService > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_it_should_return_error_message_if_an_object_already_exist_with_same_username_and_branch()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            var resultMessage = await _userCredentialService.CreateUserCredential(UserCredentialsStubs.GetExistingUserCredential());

            Assert.AreEqual("GBase user credential record already exists for the current user and branch", resultMessage);
        }

        [Test]
        [Category("UserCredentialService > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_it_should_call_Add_method_of_UserCredentialSource()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));
            _userCredentialsSourceMock.Setup(x => x.Add(It.IsAny<UserCredential>())).Returns(Task.FromResult(It.IsAny<bool>()));

            var resultMessage = await _userCredentialService.CreateUserCredential(UserCredentialsStubs.GetNewUserCredential());

            _userCredentialsSourceMock.Verify(x => x.Add(It.IsAny<UserCredential>()), Times.Once);
        }

        [Test]
        [Category("UserCredentialService > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_it_should_return_empty_message_when_usercredential_is_added_successfully()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));
            _userCredentialsSourceMock.Setup(x => x.Add(It.IsAny<UserCredential>())).Returns(Task.FromResult(true));

            var newUserCredential = UserCredentialsStubs.GetNewUserCredential();
            var resultMessage = await _userCredentialService.CreateUserCredential(newUserCredential);

            Assert.IsNullOrEmpty(resultMessage);
        }

        [Test]
        [Category("UserCredentialService > CreateUserCredential")]
        public async void When_calling_CreateUserCredential_it_should_return_failure_message_when_usercredential_addition_is_not_successful()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));
            _userCredentialsSourceMock.Setup(x => x.Add(It.IsAny<UserCredential>())).Returns(Task.FromResult(false));

            var newUserCredential = UserCredentialsStubs.GetNewUserCredential();
            var resultMessage = await _userCredentialService.CreateUserCredential(newUserCredential);

            Assert.AreEqual($"Error while adding GBase credential for {newUserCredential.UserName} for {newUserCredential.Branch} branch", resultMessage);
        }

        [Test]
        [Category("UserCredentialService > RemoveUserCredential")]
        public async void When_calling_RemoveUserCredential_it_should_call_Remove_method_of_UserCredentialSource()
        {
            _userCredentialsSourceMock.Setup(x => x.Remove(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<bool>());

            var resultMessage =
                await _userCredentialService.RemoveUserCredential(It.IsAny<string>(), It.IsAny<string>());

            _userCredentialsSourceMock.Verify(x => x.Remove(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        [Category("UserCredentialService > RemoveUserCredential")]
        public async void When_calling_RemoveUserCredential_it_should_return_failure_message_when_usercredential_removal_is_not_successful()
        {
            string branch = "London";
            string user = "User1";

            _userCredentialsSourceMock.Setup(x => x.Remove(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var resultMessage =
                await _userCredentialService.RemoveUserCredential(user, branch);

            Assert.AreEqual($"Error while removing GBase credential for {user} for {branch} branch", resultMessage);
        }

        [Test]
        [Category("UserCredentialService > RemoveUserCredential")]
        public async void When_calling_RemoveUserCredential_it_should_return_empty_message_when_usercredential_removal_is_successful()
        {
            _userCredentialsSourceMock.Setup(x => x.Remove(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var resultMessage =
                await _userCredentialService.RemoveUserCredential(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsNullOrEmpty(resultMessage);
        }

        [Test]
        [Category("UserCredentialService > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_return_error_message_if_input_object_is_null()
        {
            var resultMessage = await _userCredentialService.UpdateUserCredential(null);

            Assert.AreEqual("Invalid UserCredential Model.", resultMessage);
        }

        [Test]
        [Category("UserCredentialService > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_return_error_message_if_input_object_has_empty_properties()
        {
            var resultMessage = await _userCredentialService.UpdateUserCredential(UserCredentialsStubs.GetInvalidUserCredential());

            Assert.AreEqual("Invalid UserCredential Model. One of more required fields are missing.", resultMessage);
        }

        [Test]
        [Category("UserCredentialService > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_return_error_message_if_input_object_doesnot_exists()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));

            var resultMessage = await _userCredentialService.UpdateUserCredential(UserCredentialsStubs.GetNewUserCredential());

            Assert.AreEqual("GBase user credential record does not exists for the current user and branch", resultMessage);
        }

        [Test]
        [Category("UserCredentialService > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_call_Update_method_of_UserCredentialSource()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));
            _userCredentialsSourceMock.Setup(x => x.Update(It.IsAny<UserCredential>())).Returns(Task.FromResult(It.IsAny<bool>()));

            var resultMessage = await _userCredentialService.UpdateUserCredential(UserCredentialsStubs.GetExistingUserCredential());

            _userCredentialsSourceMock.Verify(x => x.Update(It.IsAny<UserCredential>()), Times.Once);
        }

        [Test]
        [Category("UserCredentialService > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_return_empty_message_when_usercredential_update_is_successfully()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));
            _userCredentialsSourceMock.Setup(x => x.Update(It.IsAny<UserCredential>())).Returns(Task.FromResult(true));

            var existingUserCredential = UserCredentialsStubs.GetExistingUserCredential();
            var resultMessage = await _userCredentialService.UpdateUserCredential(existingUserCredential);

            Assert.IsNullOrEmpty(resultMessage);
        }

        [Test]
        [Category("UserCredentialService > UpdateUserCredential")]
        public async void When_calling_UpdateUserCredential_it_should_return_failure_message_when_usercredential_update_is_not_successful()
        {
            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).Returns(Task.FromResult(_sourceList));
            _userCredentialsSourceMock.Setup(x => x.Update(It.IsAny<UserCredential>())).Returns(Task.FromResult(false));

            var existingUserCredential = UserCredentialsStubs.GetExistingUserCredential();
            var resultMessage = await _userCredentialService.UpdateUserCredential(existingUserCredential);

            Assert.AreEqual($"Error while updating GBase credential for {existingUserCredential.UserName} for {existingUserCredential.Branch} branch", resultMessage);
        }

        [Test]
        [Category("UserCredentialService > TestGBaseCredential")]
        public async void When_calling_TestGBaseCredential_it_should_return_error_message_if_user_credential_does_not_exists()
        {
            string username = "newusername1";
            string branch = "London";

            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).ReturnsAsync(_sourceList);

            var response = await _userCredentialService.TestGBaseCredential(username, branch);
            
            Assert.AreEqual($"User Credential doesn't exist for user {username} for branch {branch}", response);
        }

        [Test]
        [Category("UserCredentialService > TestGBaseCredential")]
        public async void When_calling_TestGBaseCredential_it_should_call_method_to_send_message()
        {
            string username = "newusername1";
            string branch = "London";

            _userCredentialsSourceMock.Setup(x => x.GetUserCredentials()).ReturnsAsync(_sourceList);

            var response = await _userCredentialService.TestGBaseCredential(username, branch);
        }
        */
    }
}
