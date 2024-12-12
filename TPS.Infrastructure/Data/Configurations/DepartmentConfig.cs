using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

public class DepartmentConfig : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(d => d.Name)
               .HasMaxLength(40)
               .IsRequired();

        builder.Property(d => d.Abbreviation)
               .HasMaxLength(4)
               .IsRequired();

        builder.HasData(
            new Department { Id = 1, Name = "Computer Science", Abbreviation = "CS" },
            new Department { Id = 2, Name = "Computer Information Systems", Abbreviation = "CIS" },
            new Department { Id = 3, Name = "Information Technology", Abbreviation = "IT" },
            new Department { Id = 4, Name = "Artificial Intelligence", Abbreviation = "AI" }
        );
    }
}
