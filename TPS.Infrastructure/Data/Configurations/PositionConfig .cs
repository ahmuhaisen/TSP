using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

internal class PositionConfig : IEntityTypeConfiguration<FacultyRank>
{
    public void Configure(EntityTypeBuilder<FacultyRank> builder)
    {
        builder.Property(p => p.Title)
               .HasMaxLength(40)
               .IsRequired();

        builder.HasData(
                new FacultyRank { Id = 1, Title = "Professor" },
                new FacultyRank { Id = 2, Title = "Associate Professor" },
                new FacultyRank { Id = 3, Title = "Assistant Professor" },
                new FacultyRank { Id = 4, Title = "Teacher" },
                new FacultyRank { Id = 5, Title = "Department Chair" },
                new FacultyRank { Id = 6, Title = "Dean" },
                new FacultyRank { Id = 7, Title = "Dean Assistant" }
            );
    }
}
