using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TSP.Domain.Entities;
using TSP.Domain.Primitives;

namespace TPS.Infrastructure.Data;


public class ApplicationDbContext(DbContextOptions options, IPublisher _publisher) : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{

    public DbSet<Student> Students { get; set; }
    public DbSet<FacultyMember> FacultyMembers { get; set; }
    public DbSet<Rank> Ranks { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Society> Societies { get; set; }
    public DbSet<SocietiesMembers> SocietiesMembers  { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventApproval> EventsApproval { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
    public DbSet<MembershipRequest> MembershipsRequests { get; set; }
    public DbSet<Notification> Notifications { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyReference).Assembly);

        base.OnModelCreating(modelBuilder);
    }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .SelectMany(e => e.DomainEvents);

        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
