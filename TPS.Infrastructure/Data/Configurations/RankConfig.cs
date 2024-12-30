using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

internal class RankConfig : IEntityTypeConfiguration<Rank>
{
    public void Configure(EntityTypeBuilder<Rank> builder)
    {
        builder.Property(p => p.Title)
               .HasMaxLength(50)
               .IsRequired();

        builder.HasData(
                new Rank { Id = 1, Title = "Dean" },
                new Rank { Id = 2, Title = "Dean Assistant" },
                new Rank { Id = 3, Title = "Department Chair" },
                new Rank { Id = 4, Title = "Professor" },
                new Rank { Id = 5, Title = "Associate Professor" },
                new Rank { Id = 6, Title = "Assistant Professor" },
                new Rank { Id = 7, Title = "Teacher" },
                new Rank { Id = 8, Title = "Secretary / Typist" },
                new Rank { Id = 9, Title = "Computer Lab Supervisor" },
                new Rank { Id = 10, Title = "Computer Engineer" },
                new Rank { Id = 11, Title = "Monitoring and Controlling" }
            );
    }
}
