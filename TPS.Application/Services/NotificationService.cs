using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz.Logging;
using System.Runtime.InteropServices;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Notifications.Contracts;
using TPS.Application.SignalR;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Shared;

namespace TPS.Application.Services;


public class NotificationService(ApplicationDbContext _context,
                                 UserManager<ApplicationUser> _userManager,
                                 IHubContext<NotificationHub> _hubContext,
                                 IUserConnectionManager _connectionManager,
                                 ILogger<NotificationService> _logger) : INotificationService
{
    public const string ReceiveNotificationKey = "ReceiveNotification";


    public async Task SendNotificationForAllUsers(string subject, string body)
    {
        _logger.BeginScope("SendNotificationForAllUsers");
        _logger.LogInformation($"Sending notification to all users...");

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

            if (connections == null || !connections.Any())
            {
                _logger.LogWarning($"No connections found for user {usr.Id}");
                continue;
            }

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

    public async Task<Result<List<NotificationDto>>> GetAllUserNotifications(Guid userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
                .ThenByDescending(n => n.IsSeen)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Subject = n.Subject,
                Body = n.Body,
                CreatedAt = n.CreatedAt,
                IsSeen = n.IsSeen,
                ImageId = n.ImageId,
            })
            .ToListAsync();
        
        return Result.Success(notifications);
    }
}
