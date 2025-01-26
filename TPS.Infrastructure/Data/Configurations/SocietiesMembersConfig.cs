using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

internal class SocietiesMembersConfig : IEntityTypeConfiguration<SocietiesMembers>
{
    public void Configure(EntityTypeBuilder<SocietiesMembers> builder)
    {
        builder.HasKey(sm => new { sm.SocietyId, sm.StudentId });

        builder.HasOne(sm => sm.Society)
               .WithMany(s => s.SocietiesMembers)
               .HasForeignKey(sm => sm.SocietyId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(sm => sm.Student)
               .WithMany(s => s.SocietiesMembers)
               .HasForeignKey(sm => sm.StudentId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.Property(sm => sm.IsCommittee)
               .HasDefaultValue(false);

    }
}
