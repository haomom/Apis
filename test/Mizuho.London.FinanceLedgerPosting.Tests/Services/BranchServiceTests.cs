using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.ModelDTO;
using Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure;
using Mizuho.London.FinanceLedgerPosting.Repository.Repositories;
using Mizuho.London.FinanceLedgerPosting.Services;
using Mizuho.London.FinanceLedgerPosting.Tests.Helper;
using Mizuho.London.FinanceLedgerPosting.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Services
{
    [TestFixture]
    public class BranchServiceTests
    {
        private Mock<IBranchRepository> _branchRepositoryMock;

        private readonly List<Branch> _sourceList = BranchesStubs.GetAllBranches().ToList();

        private Mock<FinanceLedgerPostingDbContext> _mockDbContext;
        private Mock<DbSet<Branch>> _mockDbSet;

        private BranchRepository _branchRepository;
        private BranchService _branchService;

        [SetUp]
        public void Setup()
        {
            _branchRepositoryMock = new Mock<IBranchRepository>();

            _mockDbSet = MockDBHelper.GetQueryableMockDbSet<Branch>(_sourceList, (o => o.BranchId));
            _mockDbContext = new Mock<FinanceLedgerPostingDbContext>();
            _mockDbContext.Setup(x => x.Set<Branch>()).Returns(_mockDbSet.Object);

            _branchRepository = new BranchRepository(_mockDbContext.Object);

            _branchService = new BranchService(_branchRepositoryMock.Object);
        }

        [Test]
        public async void When_calling_GetAllBranches_it_should_call_GetAllEntities_from_BranchRepository()
        {
            await _branchService.GetAllBranches();

            _branchRepositoryMock.Verify(x => x.GetAllEntities());
        }

        [Test]
        public async void When_calling_GetAllBranches_it_should_return_all_entries()
        {
            _branchService = new BranchService(_branchRepository);

            var result = await _branchService.GetAllBranches();

            Assert.AreEqual(_sourceList.Count, result.Count());

            ObjectComparer.ListOfPropertiesValuesAreEquals(_sourceList, result.ToList());
        }

        [Test]
        public async void When_calling_GetByBranchCode_it_should_return_filtered_object_if_object_exists()
        {
            _branchService = new BranchService(_branchRepository);
            var expected = _sourceList.Find(x => x.BranchCode == "LDN");

            var filteredObject = await _branchService.GetByBranchCode("LDN");

            Assert.IsNotNull(filteredObject);
            ObjectComparer.PropertyValuesAreEquals(expected, filteredObject);
        }

        [Test]
        public async void When_calling_GetByBranchCode_it_should_return_null_object_if_object_doesnnot_exists()
        {
            _branchService = new BranchService(_branchRepository);

            var filteredObject = await _branchService.GetByBranchCode("xxx");

            Assert.IsNull(filteredObject);
        }
    }
}
