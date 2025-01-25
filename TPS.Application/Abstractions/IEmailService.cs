namespace TPS.Application.Abstractions;

public interface IEmailService
{
    Task Send(string to, string subject, string body);
}