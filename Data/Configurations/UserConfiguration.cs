using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementApi.Models;

namespace TaskManagementApi.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(e => e.Email)
                .IsUnique()
                .HasDatabaseName("IX_User_Email");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}