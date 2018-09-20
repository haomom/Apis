using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace Mizuho.London.FinanceLedgerPosting.Tests.Helper
{
    public static class MockDBHelper
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList, Func<T, object> primaryKey = null) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IDbAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<T>(queryable.GetEnumerator()));

            dbSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<T>(queryable.Provider));

            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            dbSet.Setup(d => d.Remove(It.IsAny<T>())).Returns<T>((s) => sourceList.Remove(s) ? s : null);

            if (primaryKey != null)
            {
                dbSet.Setup(d => d.Find(It.IsAny<object[]>())).Returns((object[] input) =>
                    sourceList.SingleOrDefault(x => primaryKey(x).Equals(input.First())));

                dbSet.Setup(d => d.FindAsync(It.IsAny<object[]>())).Returns((object[] input) =>
                    Task.FromResult(sourceList.SingleOrDefault(x => primaryKey(x).Equals(input.First()))));
            }
            
            return dbSet.Object;
        }
    }
}
