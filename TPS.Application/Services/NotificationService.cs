using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TPS.Application.Abstractions;
using TPS.Application.SignalR;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;

namespace TPS.Application.Services;


public class NotificationService(ApplicationDbContext _context,
                                 IHubContext<NotificationHub> _hubContext,
                                 IUserConnectionManager _connectionManager) : INotificationService
{
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

        _context.SaveChanges();

        foreach (var std in students)
        {
            var connections = _connectionManager.GetConnections(std.Id.ToString());

            foreach (var connectionId in connections)
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", new
                {
                    Subject = subject.Trim(),
                    Body = body.Trim(),
                    CreatedAt = DateTime.Now
                });
            }
        }
    }
}
