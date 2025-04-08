using Microsoft.EntityFrameworkCore;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations
{
    public class EventApprovalConfig : IEntityTypeConfiguration<EventApproval>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<EventApproval> builder)
        {
            builder.Property(d => d.DeanAssistantApproval)
                .IsRequired(false);
            builder.Property(d => d.AdvisorApproval)
                .IsRequired(false);

            builder.Property(d => d.Remarks)
                .HasMaxLength(250)
                .IsRequired(false);
            builder.HasOne(s => s.Event)
                .WithMany()
                .HasForeignKey(d => d.EventId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s=>s.FacultyMember)
                .WithMany()
                .HasForeignKey(d=>d.FacultyMemberId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
