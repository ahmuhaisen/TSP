using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(fm => fm.FirstName)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(fm => fm.LastName)
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(a => a.Gender)
               .HasConversion<string>()
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(a => a.ProfileImageId)
               .HasMaxLength(50)
               .IsRequired(false);
    }
}