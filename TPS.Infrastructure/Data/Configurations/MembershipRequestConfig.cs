using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;


public class MembershipRequestConfig : IEntityTypeConfiguration<MembershipRequest>
{
    public void Configure(EntityTypeBuilder<MembershipRequest> builder)
    {
        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.HasOne(x => x.Society)
            .WithMany(x => x.MembershipRequests)
            .HasForeignKey(x => x.SocietyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Student)
            .WithMany(x => x.MembershipRequests)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Section)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Motivation)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(x => x.RequestedOn)
            .IsRequired();
    }
}
