using System.ComponentModel.DataAnnotations.Schema;
using Mizuho.London.FinanceLedgerPosting.Data.Entities;
using System.Data.Entity.ModelConfiguration;
using Mizuho.London.FinanceLedgerPosting.Data.Extensions;

namespace Mizuho.London.FinanceLedgerPosting.Data.Map
{
    public class UserCredentialMapping : EntityTypeConfiguration<UserCredential>
    {
        public UserCredentialMapping()
        {
            ToTable("FLP_UserCredential");

            HasKey(m => m.UserCredentialId);

            Property(p => p.UserCredentialId)
                .HasColumnName("UserCredentialId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(p => p.UserName)
                .HasColumnName("UserName")
                .HasMaxLength(15)
                .IsRequired()
                .HasUniqueIndexAnnotation("IX_UserCredential_UserName_Branch", 0);

            Property(p => p.Branch)
                .HasColumnName("Branch")
                .HasMaxLength(5)
                .IsRequired()
                .HasUniqueIndexAnnotation("IX_UserCredential_UserName_Branch", 1);

            Property(p => p.GBaseUserId)
                .HasColumnName("GBaseUserId")
                .HasMaxLength(10)
                .IsRequired();

            Property(p => p.GBaseEmployeeId)
                .HasColumnName("GBaseEmployeeId")
                .HasMaxLength(10)
                .IsRequired();

            Property(p => p.GBasePassword)
                .HasColumnName("GBasePassword")
                .IsRequired();

            Property(p => p.ExpiryDate)
                .HasColumnName("ExpiryDate")
                .IsRequired();

            Property(p => p.ModifiedBy)
                .HasColumnName("ModifiedBy")
                .HasMaxLength(15)
                .IsRequired();

            Property(p => p.CreatedOn)
                .HasColumnName("CreatedOn")
                .IsRequired();

            Property(p => p.LastModifiedOn)
                .HasColumnName("LastModifiedOn")
                .IsRequired();
        }
    }
}
