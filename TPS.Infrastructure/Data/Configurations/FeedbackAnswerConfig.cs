using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

public class FeedbackAnswerConfig : IEntityTypeConfiguration<FeedbackAnswer>
{
    public void Configure(EntityTypeBuilder<FeedbackAnswer> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Rating)
            .HasPrecision(2, 1)
            .IsRequired();

        builder.Property(f => f.Notes)
            .HasMaxLength(1500);

        builder.Property(f => f.SubmittedAt)
            .IsRequired();

        builder.HasOne(f => f.Event)
            .WithMany(e => e.FeedbackAnswers)
            .HasForeignKey(f => f.EventId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}