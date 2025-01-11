using Microsoft.EntityFrameworkCore;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations
{
    public class EventApprovalConfig : IEntityTypeConfiguration<EventApproval>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<EventApproval> builder)
        {
            builder.Property(d => d.IsApproved)
                .IsRequired();
            builder.Property(d => d.Remarks)
                .HasMaxLength(250)
                .IsRequired(false);
            builder.HasOne(s => s.Event)
                .WithMany()
                .HasForeignKey(d => d.EventId)
                .IsRequired();
            builder.HasOne(s=>s.FacultyMember)
                .WithMany()
                .HasForeignKey(d=>d.FacultyMemberId)
                .IsRequired();
        }
    }
}
