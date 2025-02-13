using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.Property(d => d.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.Description)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(d => d.type)
                .IsRequired(false);

            builder.HasOne(s => s.Society)
                .WithMany(s=>s.Events)
                .HasForeignKey(d => d.SocietyId)
                .IsRequired();

            builder.HasOne(s => s.Student)
                .WithMany(s => s.RequestedEvents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
