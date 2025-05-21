using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;
using TPS.Application.Areas.Shared.Notifications.Contracts;
using TPS.Application.SignalR;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;
using TSP.Domain.Enums;
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


    public async Task<Result> MarkNotificationAsReadAsync(Guid notificationId, Guid userId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

        if (notification is null)
            return Result.Failure(Error.NotFound(nameof(Notification), notificationId.ToString()));

        notification.IsSeen = true;

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result> MarkAllNotificationsAsReadAsync(Guid userId)
    {
        var notifications = await _context.Notifications
            .Where(n => n.UserId == userId && !n.IsSeen)
            .ToListAsync();

        if (notifications is null || notifications.Count == 0)
            return Result.Failure(Error.NotFound(nameof(Notification), userId.ToString()));

        foreach (var notification in notifications)
        {
            notification.IsSeen = true;
        }

        await _context.SaveChangesAsync();

        return Result.Success();
    }

    public async Task SendNotificationToUser(Guid userId, UserType userType, string subject, string body)
    {
        if (userType == UserType.Student)
        {
            var student = await _context.Students
                .Where(x => x.Id == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = student.Id,
                Subject = subject.Trim(),
                Body = body.Trim(),
                CreatedAt = DateTime.Now
            };
            await _context.Notifications.AddAsync(notification);
            var connection = _connectionManager.GetConnections(student.Id.ToString());
            foreach(var connectionId in connection)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync(ReceiveNotificationKey, new
                {
                    Subject = subject.Trim(),
                    Body = body.Trim(),
                    CreatedAt = DateTime.Now
                });
            }
        }
        if (userType == UserType.FacultyMember)
        {
            var facultyMember = await _context.FacultyMembers
                .Where(x => x.Id == userId)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                UserId = facultyMember.Id,
                Subject = subject.Trim(),
                Body = body.Trim(),
                CreatedAt = DateTime.Now
            };
            await _context.Notifications.AddAsync(notification);
            var connection = _connectionManager.GetConnections(facultyMember.Id.ToString());
            foreach(var connectionId in connection)
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

    public async Task SendNotificationToSocietyMembers(Guid societyId, string subject, string body)
    {
        var societyMembers = await _context.SocietiesMembers
            .Where(x => societyId == x.SocietyId)
            .AsNoTracking()
            .ToListAsync();

        var notifications = societyMembers.Select(std => new Notification
        {
            Id = Guid.NewGuid(),
            UserId = std.StudentId,
            Subject = subject.Trim(),
            Body = body.Trim(),
            CreatedAt = DateTime.Now
        }).ToList();

        await _context.Notifications.AddRangeAsync(notifications);

        foreach (var std in societyMembers)
        {
            var connections = _connectionManager.GetConnections(std.StudentId.ToString());

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
