using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using TPS.Application.Abstractions;
using TPS.Application.SignalR;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;

namespace TPS.Application.Services;


public class NotificationService(ApplicationDbContext _context,
                                 UserManager<ApplicationUser> _userManager,
                                 IHubContext<NotificationHub> _hubContext,
                                 IUserConnectionManager _connectionManager) : INotificationService
{
    public const string ReceiveNotificationKey = "ReceiveNotification";


    public async Task SendNotificationForAllUsers(string subject, string body)
    {
        var users = await _userManager.Users.ToListAsync();

        var notifications = users.Select(std => new Notification
        {
            Id = Guid.NewGuid(),
            UserId = std.Id,
            Subject = subject.Trim(),
            Body = body.Trim(),
            CreatedAt = DateTime.Now
        }).ToList();

        await _context.Notifications.AddRangeAsync(notifications);

        await _context.SaveChangesAsync();

        foreach (var usr in users)
        {
            var connections = _connectionManager.GetConnections(usr.Id.ToString());

            foreach (var connectionId in connections)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync(ReceiveNotificationKey, new
                {
                    Subject = subject.Trim(),
                    Body = body.Trim(),
                    CreatedAt = DateTime.Now
                });
            }
        }
    }

    public async Task SendNotificationForAllStudents(string subject, string body)
    {
        var students = await _context.Students.AsNoTracking().ToListAsync();

        var notifications = students.Select(std => new Notification
        {
            Id = Guid.NewGuid(),
            UserId = std.Id,
            Subject = subject.Trim(),
            Body = body.Trim(),
            CreatedAt = DateTime.Now
        }).ToList();

        await _context.Notifications.AddRangeAsync(notifications);

        foreach (var std in students)
        {
            var connections = _connectionManager.GetConnections(std.Id.ToString());

            foreach (var connectionId in connections)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync(ReceiveNotificationKey, new
                {
                    Subject = subject.Trim(),
                    Body = body.Trim(),
                    CreatedAt = DateTime.Now
                });
            }
        }
    }

    public async Task SendNotificationForAllFacultyMembers(string subject, string body)
    {
        var facultyMembers = await _context.FacultyMembers.AsNoTracking().ToListAsync();

        var notifications = facultyMembers.Select(std => new Notification
        {
            Id = Guid.NewGuid(),
            UserId = std.Id,
            Subject = subject.Trim(),
            Body = body.Trim(),
            CreatedAt = DateTime.Now
        }).ToList();

        await _context.Notifications.AddRangeAsync(notifications);

        foreach (var fm in facultyMembers)
        {
            var connections = _connectionManager.GetConnections(fm.Id.ToString());

            foreach (var connectionId in connections)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync(ReceiveNotificationKey, new
                {
                    Subject = subject.Trim(),
                    Body = body.Trim(),
                    CreatedAt = DateTime.Now
                });
            }
        }
    }
}
