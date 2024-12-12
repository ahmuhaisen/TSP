using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

public class SocietyConfig : IEntityTypeConfiguration<Society>
{
    public void Configure(EntityTypeBuilder<Society> builder)
    {
        builder.Property(s => s.Name)
               .HasMaxLength(70)
               .IsRequired();

        builder.Property(s => s.Description)
               .HasMaxLength(200)
               .IsRequired();

        builder.Property(s => s.LogoId)
               .IsRequired();

        builder.Property(s => s.CreationDate)
               .IsRequired();

        builder.Property(s => s.ThemeColor)
               .HasMaxLength(7);

        builder.HasOne(s => s.Advisor)
               .WithMany(fm => fm.SocietiesAdvised)
               .HasForeignKey(s => s.AdvisorId)
               .IsRequired();
    }
}
