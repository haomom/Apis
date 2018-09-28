using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using Mizuho.London.FinanceLedgerPosting.Tests.Helper;
using Mizuho.London.FinanceLedgerPosting.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Repositories
{
    [TestFixture]
    public class SuspenseAccountRepositoryTests
    {
        private Mock<ISuspenseAccountRepository> _mockRepository;
        private Mock<SuspenseAccount> _mockSuspenseAccount;
        private Mock<FinanceLedgerPostingDbContext> _mockDbContext;
        private Mock<DbSet<SuspenseAccount>> _mockDbSet;
        private Mock<DbSet<SuspenseAccount>> _dbSetMock;
        private DbSet<SuspenseAccount> _dbSet;

        private List<SuspenseAccount> _sourceList;
        private SuspenseAccount _suspenseAccount;
        private SuspenseAccount _existingSuspenseAccount;

        [SetUp]
        public void Setup()
        {
            _sourceList = SuspenceAccountsStubs.GetSuspenseAccounts();
            _mockRepository = new Mock<ISuspenseAccountRepository>();
            _mockSuspenseAccount = new Mock<SuspenseAccount>();
            _mockDbContext = new Mock<FinanceLedgerPostingDbContext>();
            _mockDbSet = new Mock<DbSet<SuspenseAccount>>();
            _dbSetMock = MockDBHelper.GetQueryableMockDbSet(_sourceList, (o => o.SuspenseAccountId));
            _dbSet = _dbSetMock.Object;

            _suspenseAccount = SuspenceAccountsStubs.GetNewSuspenseAccount();
            _existingSuspenseAccount = SuspenceAccountsStubs.GetExistingSuspenseAccount();
        }

        [Test]
        public void SuspenseAccountRepository_Add_Method_Adds_A_New_Object()
        {
            _mockDbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(_dbSet);
            
            var suspenseAccountRepository = new SuspenseAccountRepository(_mockDbContext.Object);
            suspenseAccountRepository.Add(_suspenseAccount);

            _mockDbContext.Verify(x => x.Set<SuspenseAccount>());
            Assert.AreEqual(true, _dbSet.Any(x => x.SuspenseAccountId == 4));
            Assert.AreEqual(4, _dbSet.Count());
        }

        [Test]
        public void SuspenseAccountRepository_Add_Method_Calls_Add_Method_of_DbSet()
        {
            _mockDbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(_mockDbSet.Object);

            var suspenseAccountRepository = new SuspenseAccountRepository(_mockDbContext.Object);
            suspenseAccountRepository.Add(It.IsAny<SuspenseAccount>());

            _mockDbContext.Verify(x => x.Set<SuspenseAccount>());
            _mockDbSet.Verify(x => x.Add(It.IsAny<SuspenseAccount>()), Times.Once);
        }

        [Test]
        public async Task SuspenseAccountRepository_Remove_Method_Removes_Object()
        {
            _mockDbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(_dbSet);

            var suspenseAccountRepository = new SuspenseAccountRepository(_mockDbContext.Object);
            var obj = await suspenseAccountRepository.GetById(1);
            suspenseAccountRepository.Remove(obj);

            _mockDbContext.Verify(x => x.Set<SuspenseAccount>());
            Assert.AreEqual(false, _dbSet.Any(x => x.SuspenseAccountId == 1));
        }

        [Test]
        public void SuspenseAccountRepository_Remove_Method_Calls_Remove_Method_of_DbSet()
        {
            _mockDbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(_mockDbSet.Object);

            var suspenseAccountRepository = new SuspenseAccountRepository(_mockDbContext.Object);
            suspenseAccountRepository.Remove(It.IsAny<SuspenseAccount>());

            _mockDbContext.Verify(x => x.Set<SuspenseAccount>());
            _mockDbSet.Verify(x => x.Remove(It.IsAny<SuspenseAccount>()), Times.Once);
        }

        [Test]
        public async Task SuspenseAccountRepository_GetById_Method_Returns_SuspenseAccount_Object()
        {
            _mockDbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(_dbSet);

            var suspenseAccountRepository = new SuspenseAccountRepository(_mockDbContext.Object);
            SuspenseAccount response = await suspenseAccountRepository.GetById(1);

            _mockDbContext.Verify(x => x.Set<SuspenseAccount>());
            ObjectComparer.PropertyValuesAreEquals(response, _existingSuspenseAccount);
        }

        [Test]
        public async Task SuspenseAccountRepository_GetById_Method_Calls_Find_Method_of_DbSet()
        {
            _mockDbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(_mockDbSet.Object);

            var suspenseAccountRepository = new SuspenseAccountRepository(_mockDbContext.Object);
            await suspenseAccountRepository.GetById(It.IsAny<int>());

            _mockDbContext.Verify(x => x.Set<SuspenseAccount>());
            _mockDbSet.Verify(x => x.FindAsync(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void SuspenseAccountRepository_GetAll_Method_Returns_All_Elements_of_List()
        {
            _mockDbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(_dbSet);

            var suspenseAccountRepository = new SuspenseAccountRepository(_mockDbContext.Object);
            var result = suspenseAccountRepository.GetAll().ToList();

            Assert.AreEqual(_sourceList.Count, result.Count());
            ObjectComparer.ListOfPropertiesValuesAreEquals(result, _sourceList);
        }

        [Test]
        public void SuspenseAccountRepository_Update_Method_Calls_Attach_Method_Of_DbSet()
        {
            _mockDbContext.Setup(x => x.Set<SuspenseAccount>()).Returns(_mockDbSet.Object);

            var suspenseAccountRepository = new SuspenseAccountRepository(_mockDbContext.Object);
            suspenseAccountRepository.Update(It.IsAny<SuspenseAccount>());

            _mockDbContext.Verify(x => x.Set<SuspenseAccount>());
            _mockDbSet.Verify(x => x.Attach(It.IsAny<SuspenseAccount>()), Times.Once);
        }
    }
}
