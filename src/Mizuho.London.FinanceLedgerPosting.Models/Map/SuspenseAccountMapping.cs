using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;

namespace Mizuho.London.FinanceLedgerPosting.Data.Map
{
    public class SuspenseAccountMapping : EntityTypeConfiguration<SuspenseAccount>
    {
        public SuspenseAccountMapping()
        {
            ToTable("SuspenseAccount");

            HasKey(m => m.SuspenseAccountId);

            Property(p => p.SuspenseAccountId).
               HasColumnName("SuspenseAccountId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.Branch).
                HasColumnName("Branch").
                IsRequired();

            Property(p => p.Currency).
                HasColumnName("Currency").
                HasMaxLength(4).
                IsRequired();

            Property(p => p.AccountCode).
                HasColumnName("AccountCode").
                HasMaxLength(5).
                IsRequired();

            Property(p => p.AccountNoPart1).
                HasColumnName("AccountNoPart1").
                HasMaxLength(3).
                IsRequired();

            Property(p => p.AccountNoPart2).
                HasColumnName("AccountNoPart2").
                HasMaxLength(6).
                IsRequired();
        }
    }
}
