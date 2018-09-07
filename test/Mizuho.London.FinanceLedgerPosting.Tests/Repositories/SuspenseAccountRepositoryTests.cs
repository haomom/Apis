using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Repositories
{
    [TestFixture]
    public class SuspenseAccountRepositoryTests
    {
        private Mock<ISuspenseAccountRepository> _mockRepository;
        private Mock<SuspenseAccount> _mockSuspenseAccount;

        private SuspenseAccount _suspenseAccount;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<ISuspenseAccountRepository>();
            _mockSuspenseAccount = new Mock<SuspenseAccount>();

            _suspenseAccount = new SuspenseAccount
            {
                SuspenseAccountId = 1,
                Branch = "xxx",
                Currency = "xxx",
                AccountCode = "111",
                AccountNoPart1 = "1234",
                AccountNoPart2 = "5667",
            };
        }

        [Test]
        public void Create_SuspenseAccount_Calls_SuspenseAccountRepository_Add_Method()
        {
            _mockRepository.Setup(x => x.Add(_mockSuspenseAccount.Object));
            _mockRepository.Object.Add(_mockSuspenseAccount.Object);
            _mockRepository.Verify(x => x.Add(_mockSuspenseAccount.Object), Times.Once());
        }
    }
}
