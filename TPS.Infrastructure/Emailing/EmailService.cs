using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using TSP.Domain.Shared.Options;
using TPS.Infrastructure.Emailing.Templates;
using System.Reflection;
using TSP.Domain.Enums;

namespace TPS.Infrastructure.Emailing;


public class EmailService : IEmailService
{
    private readonly IOptions<EmailOptions> options;

    public EmailService(IOptions<EmailOptions> options)
    {
        this.options = options;
    }

    public async Task Send(string to, string subject, string body)
    {
        var email = options.Value.Email;
        var password = options.Value.Password;
        var host = options.Value.Host;
        var port = options.Value.Port;

        var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(email, password)
        };

        var emailBody = EmailLayout.Text
            .Replace("{{body}}", body)
            .Replace("{{year}}", DateTime.Now.Year.ToString());

        var message = new MailMessage
        {
            From = new MailAddress(email),
            To = { to },
            Subject = subject,
            Body = emailBody,
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }

    public async Task SendWelcomingEmail(string to, string userName, UserType userType)
    {
        string body;

        if (userType == UserType.Student)
        {
            body = WelcomingEmail.StudentText
                .Replace("{{userName}}", userName);
        }
        else if (userType == UserType.FacultyMember)
        {
            body = WelcomingEmail.FacultyMemberText
                .Replace("{{userName}}", userName);
        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }

        var subject = "Welcome to The Societies Portal!";

        await Send(to, subject, body);
    }
}

