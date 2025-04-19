namespace TPS.Application.Abstractions;


public interface INotificationService
{
    Task SendNotificationForAllUsers(string subject, string body);
    Task SendNotificationForAllFacultyMembers(string subject, string body);
    Task SendNotificationForAllStudents(string subject, string body);
}
