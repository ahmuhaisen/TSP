using TSP.Domain.Enums;

namespace TPS.Infrastructure.Emailing;

public interface IEmailService
{
    Task Send(string to, string subject, string body);
    Task SendWelcomingEmail(string to, string userName, UserType userType);
}