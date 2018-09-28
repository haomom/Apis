using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;

namespace Mizuho.London.FinanceLedgerPosting.Data.Map
{
    public class BranchMapping : EntityTypeConfiguration<Branch>
    {
        public BranchMapping()
        {
            ToTable("FLP_Branch");

            HasKey(m => m.BranchId);

            Property(p => p.BranchId)
                .HasColumnName("BranchId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.BranchName)
                .HasColumnName("BranchName")
                .HasMaxLength(20)
                .IsRequired();

            Property(p => p.BranchCode)
                .HasColumnName("BranchCode")
                .HasMaxLength(5)
                .IsRequired();

            Property(p => p.BranchAccountCode)
                .HasColumnName("BranchAccountCode")
                .HasMaxLength(5)
                .IsRequired();
        }
    }
}
