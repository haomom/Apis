using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Mizuho.London.FinanceLedgerPosting.Common.Data;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity.ModelConfiguration;
//using Mizuho.London.FinanceLedgerPosting.Common.Data;


namespace Mizuho.London.FinanceLedgerPosting.Repository.Map
{
    internal class SuspenseAccountMapping : EntityTypeConfiguration<SuspenseAccount>
    {
        internal SuspenseAccountMapping()
        {
            ToTable("SuspenseAccountConfiguration");

            HasKey(m => m.Id);

            Property(p => p.Id).
               HasColumnName("Id").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //IsRequired().

            Property(p => p.BranchNo).
                HasColumnName("BranchNo").
                HasMaxLength(4).
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



            //Property(p => p.Id).
            //    HasColumnName("Id").
            //    IsRequired().
            //    HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            
            //Property(p => p.Id).
            //    HasColumnName("Id");
            // HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


        }
    }
}
