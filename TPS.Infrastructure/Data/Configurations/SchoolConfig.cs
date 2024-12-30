using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

public class SchoolConfig : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.Property(s => s.Name)
               .HasMaxLength(60)
               .IsRequired();

        builder.HasMany(s => s.Departments)
               .WithOne(d => d.School)
               .HasForeignKey(d => d.SchoolId)
               .IsRequired();
        
        builder.HasData(
            new School { Id = 1, Name = "King Abdullah II School of Information Technology" },
            new School { Id = 2, Name = "School of Engineering" },
            new School { Id = 3, Name = "School of Science" },
            new School { Id = 4, Name = "School of Agriculture" },
            new School { Id = 5, Name = "School of Medicine" },
            new School { Id = 6, Name = "School of Dentistry" },
            new School { Id = 7, Name = "School of Pharmacy" },
            new School { Id = 8, Name = "School of Nursing" },
            new School { Id = 9, Name = "School of Rehabilitation Sciences" },
            new School { Id = 10, Name = "School of Arts" },
            new School { Id = 11, Name = "School of Business" },
            new School { Id = 12, Name = "School of Sharia" },
            new School { Id = 13, Name = "School of Sport Science" },
            new School { Id = 14, Name = "School of Law" },
            new School { Id = 15, Name = "School of Physical Education" },
            new School { Id = 16, Name = "School of Arts and Design" },
            new School { Id = 17, Name = "School of Political Science and International Studies" },
            new School { Id = 18, Name = "School of Foreign Languages" },
            new School { Id = 19, Name = "School of Archaeology and Tourism" }
        );

    }
}