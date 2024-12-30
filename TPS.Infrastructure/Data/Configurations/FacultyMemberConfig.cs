using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data.Configurations;

public class FacultyMemberConfig : IEntityTypeConfiguration<FacultyMember>
{
    public void Configure(EntityTypeBuilder<FacultyMember> builder)
    {
        builder.ToTable("FacultyMembers")
               .HasBaseType<ApplicationUser>();

        builder.Property(fm => fm.EmployeeNumber)
               .HasMaxLength(20)
               .IsRequired();
        
        builder.HasOne(fm => fm.Rank)
               .WithMany()
               .HasForeignKey(fm => fm.RankId)
               .IsRequired();
    }
}
