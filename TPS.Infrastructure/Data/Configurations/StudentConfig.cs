using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students")
               .HasBaseType<ApplicationUser>();

        builder.Property(s => s.UniversityNumber)
               .HasMaxLength(8)
               .IsRequired();
    }
}
