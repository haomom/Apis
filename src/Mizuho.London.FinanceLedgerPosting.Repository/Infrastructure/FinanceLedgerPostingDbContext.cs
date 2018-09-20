﻿using System.Data.Entity;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using Mizuho.London.FinanceLedgerPosting.Data.Map;
using Mizuho.London.FinanceLedgerPosting.Repository.Interfaces;

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

        void IUnitOfWork.CommitAsync()
        {
            base.SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new SuspenseAccountMapping());
            base.OnModelCreating(modelBuilder);
        }
    }
}
