namespace TPS.Application.Abstractions;


public interface INotificationService
{
    Task SendNotificationForAllStudents(string subject, string body);
}
