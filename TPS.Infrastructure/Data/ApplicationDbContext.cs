using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public DbSet<Student> Students { get; set; }
    public DbSet<FacultyMember> FacultyMembers { get; set; }
    public DbSet<Rank> Ranks { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Society> Societies { get; set; }
    public DbSet<SocietiesMembers> SocietiesMembers  { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
