using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;


public class NotificationConfig : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.Property(n => n.Subject)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(n => n.Body)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(n => n.ImageId)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(n => n.CreatedAt)
           .HasDefaultValueSql("getdate()")
           .IsRequired();

        builder.Property(n => n.SeenAt)
            .HasDefaultValue(null)
            .IsRequired(false);

        builder.HasOne(n => n.ApplicationUser)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
