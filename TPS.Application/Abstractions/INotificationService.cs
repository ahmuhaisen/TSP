using TPS.Application.Areas.Shared.Notifications.Contracts;
using TSP.Domain.Enums;
using TSP.Domain.Shared;

namespace TPS.Application.Abstractions;


public interface INotificationService
{
    Task SendNotificationForAllUsers(string subject, string body);
    Task SendNotificationForAllFacultyMembers(string subject, string body);
    Task SendNotificationForAllStudents(string subject, string body);
    Task<Result<List<NotificationDto>>> GetAllUserNotifications(Guid userId);
    Task<Result> MarkNotificationAsReadAsync(Guid notificationId, Guid userId);
    Task<Result> MarkAllNotificationsAsReadAsync(Guid userId);
    Task SendNotificationToUser(Guid userId,UserType userType,string subject, string body);
    Task SendNotificationToSocietyMembers(Guid societyId, string subject, string body);
}
