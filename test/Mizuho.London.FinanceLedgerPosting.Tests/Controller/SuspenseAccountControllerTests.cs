using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using Mizuho.London.FinanceLedgerPosting.WebApi.Controllers;
using Moq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using NUnit.Framework;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Controller
{
    [TestFixture]
    public class SuspenseAccountControllerTests
    {
        private Mock<ISuspenseAccountRepository> _mockSuspenseAccountRepository;

        [SetUp]
        public void Setup()
        {
            _mockSuspenseAccountRepository = new Mock<ISuspenseAccountRepository>();
        }

        [Test]
        public void When_calling_Get_method_GetById_of_SuspenseAccountRepository_should_get_called()
        {
            _mockSuspenseAccountRepository.Setup(x => x.GetById(It.IsAny<int>()));

            SuspenceAccountController api = new SuspenceAccountController(_mockSuspenseAccountRepository.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage()
                {
                    Properties = {{HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration()}}
                }
            };

            var response = api.Get(It.IsAny<int>());

            _mockSuspenseAccountRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once());
        }
    }
}
