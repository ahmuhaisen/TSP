using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;


public class FeedbackSummaryConfig : IEntityTypeConfiguration<FeedbackSummary>
{
    public void Configure(EntityTypeBuilder<FeedbackSummary> builder)
    {
        builder.Property(x => x.AverageRating)
            .HasPrecision(3, 2);

        builder.Property(x => x.Topics)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(x => x.AiSummary)
            .HasMaxLength(2000)
            .IsRequired(false);

        builder.Property(x => x.CalculatedAt)
            .HasDefaultValueSql("getdate()")
            .ValueGeneratedOnAddOrUpdate()
            .IsRequired();
    }
}
