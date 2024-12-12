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
        
        builder.HasOne(fm => fm.Rank)
               .WithMany()
               .HasForeignKey(fm => fm.RankId)
               .IsRequired();

        builder.HasOne(fm => fm.Department)
               .WithMany()
               .HasForeignKey(fm => fm.DepartmentId)
               .IsRequired();
    }
}
