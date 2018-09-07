using Mizuho.London.FinanceLedgerPosting.Common.Data;
using Mizuho.London.FinanceLedgerPosting.Repository.Map;
using System.Data.Entity;
using Mizuho.London.FinanceLedgerPosting.Interfaces.Infrastructure;

namespace Mizuho.London.FinanceLedgerPosting.Repository.Infrastructure
{
    public class FinanceLedgerPostingDbContext : DbContext, IUnitOfWork
    {
        #region DbSet
       
        public virtual IDbSet<SuspenseAccount> SuspenseAccounts { get; set; }
       
        #endregion

        public FinanceLedgerPostingDbContext()
                : base("Name=FinanceLedgerPostingConnStr")
        { }

        void IUnitOfWork.Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SuspenseAccountMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
