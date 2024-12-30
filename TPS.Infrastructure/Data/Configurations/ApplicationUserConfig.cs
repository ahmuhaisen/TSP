using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(fm => fm.FirstName)
               .HasMaxLength(30)
               .IsRequired();

        builder.Property(fm => fm.LastName)
               .HasMaxLength(30)
               .IsRequired();

        builder.Property(a => a.Gender)
               .HasConversion<string>()
               .HasMaxLength(6)
               .IsRequired();

        builder.Property(a => a.ProfileImageId)
               .HasMaxLength(100)
               .IsRequired(false);

        builder.HasOne(a => a.Department)
               .WithMany(d => d.Users)
               .HasForeignKey(a => a.DepartmentId)
               .IsRequired(false);
    }
}