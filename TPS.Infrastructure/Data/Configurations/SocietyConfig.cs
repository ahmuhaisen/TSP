using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

public class SocietyConfig : IEntityTypeConfiguration<Society>
{
    public void Configure(EntityTypeBuilder<Society> builder)
    {
        builder.Property(s => s.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(s => s.Description)
               .HasMaxLength(250)
               .IsRequired();

        builder.Property(s => s.LogoId)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(s => s.CreationDate)
               .IsRequired();

        builder.Property(s => s.ThemeColor)
               .HasMaxLength(7);

        builder.HasOne(s => s.Advisor)
               .WithMany(fm => fm.SocietiesAdvised)
               .HasForeignKey(s => s.AdvisorId)
               .OnDelete(DeleteBehavior.Cascade)
               .IsRequired();
    }
}
