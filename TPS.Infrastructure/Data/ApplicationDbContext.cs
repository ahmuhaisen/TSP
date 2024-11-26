using Microsoft.EntityFrameworkCore;
using TSP.Domain.Entities;

namespace TPS.Infrastructure.Data;


public class ApplicationDbContext : DbContext
{
    public DbSet<Society> Societies { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
