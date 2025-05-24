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

        //await client.SendMailAsync(message);
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

    public async Task SendCommitteeChangesAlert(
        string to,
        string userName,
        UserType userType,
        string societyName,
        string committeeName,
        bool isSameUser)
    {
        string body;
        if (userType == UserType.Student)
        {
            if (isSameUser) 
            {
                body = CommiteeChangeEmail.SelfUserText
                    .Replace("{{userName}}", committeeName)
                    .Replace("{{societyName}}", societyName);
            }
            else
            {
                body = CommiteeChangeEmail.StudentText
                    .Replace("{{userName}}", userName)
                    .Replace("{{societyName}}", societyName)
                    .Replace("{{committeeName}}",committeeName);
            }
        }
        else if (userType == UserType.FacultyMember)
        {
            body = CommiteeChangeEmail.FacultyMemberText
                    .Replace("{{userName}}", userName)
                    .Replace("{{societyName}}", societyName)
                    .Replace("{{committeeName}}", committeeName);
        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }
        var subject = "Society's Committee Update";
        await Send(to, subject, body);
    }
    public async Task SendNewSocietyCreatedAlert(string to, string userName, UserType userType, string societyName)
    {
        string body;

        if (userType == UserType.Student)
        {
            body = NewSocietyCreatedEmail.StudentText
                .Replace("{{userName}}", userName)
                .Replace("{{societyName}}", societyName);
        }
        else if (userType == UserType.FacultyMember)
        {
            body = NewSocietyCreatedEmail.FacultyMemberText
                .Replace("{{userName}}", userName)
                .Replace("{{societyName}}", societyName);
        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }

        var subject = "Great News: A New Society Unleashed!";

        await Send(to, subject, body);
    }
    public async Task SendEventRequestDecisionMade(string to,
                                                   string userName, 
                                                   UserType userType, 
                                                   string societyName, 
                                                   string eventName, 
                                                   bool decision,
                                                   string? remark)
    {
        string body;

        if (userType == UserType.Student)
        {
            if (decision)
            {
                body = EventRequestDecisionEmail.AcceptText
                    .Replace("{{userName}}", userName)
                    .Replace("{{eventName}}", eventName);
            }
            else
            {
                body = EventRequestDecisionEmail.RejectText
                    .Replace("{{userName}}", userName)
                    .Replace("{{eventName}}", eventName)
                    .Replace("{{remark}}",remark);
            }
        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }

        var subject ="Update On "+ societyName + " Event Request";

        await Send(to, subject, body);
    }
    public async Task SendNewEventScheduled(string to, string userName, UserType userType,string societyName, string eventName)
    {
        string body;

        if (userType == UserType.Student)
        {
            body = NewEventScheduledEmail.StudentText
                .Replace("{{userName}}", userName)
                .Replace("{{eventName}}", eventName);
        }
        else if (userType == UserType.FacultyMember)
        {
            body = NewEventScheduledEmail.FacultyMemberText
                .Replace("{{userName}}", userName)
                .Replace("{{eventName}}", eventName);
        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }

        var subject = "Exciting News! " + societyName + " Scheduled a New Event";

        await Send(to, subject, body);
    }
    public async Task SendNewEventRequestSubmittedAlert(string to, string userName, UserType userType, string societyName, string eventName)
    {
        string body;
        if (userType == UserType.Student)
        {
            body = EventRequestSubmittedEmail.StudentText
                .Replace("{{userName}}", userName)
                .Replace("{{eventName}}", eventName);
        }
        else if (userType == UserType.FacultyMember)
        {
            body = EventRequestSubmittedEmail.FacultyMemberText
                .Replace("{{userName}}", userName)
                .Replace("{{eventName}}", eventName);
        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }

        var subject = societyName + " Update: New Event Request Submitted";
        await Send(to, subject, body);
    }
    public async Task SendMemberLeftTheSocietyAlert(string to, string userName, UserType userType, string societyName, string userNameLeft)
    {
        string body;
        if (userType == UserType.Student||userType==UserType.FacultyMember)
        {
            body = MemberLeftSocietyEmail.UserText
                .Replace("{{userNameLeft}}",userNameLeft)
                .Replace("{{societyName}}",societyName);

        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }
        var subject = societyName + " Update: A Member Has Left";
        await Send(to,subject, body);
    }
    public async Task SendNewMemberJoinedTheSocietyAlert(string to, string userName, UserType userType, string societyName, string userNameJoined)
    {
        string body;
        if (userType == UserType.Student || userType == UserType.FacultyMember)
        {
            body = MemberJoinedSocietyEmail.UserText
                .Replace("{{userNameJoined}}", userNameJoined)
                .Replace("{{societyName}}", societyName);

        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }
        var subject = societyName + " Update: A Member Has Joined!";
        await Send(to, subject, body);
    }
    public async Task SendSocietyAdvisorChangedAlert(string to,
                                                     string userName,
                                                     UserType userType,
                                                     string societyName,
                                                     string newAdvisorName,
                                                     string oldAdvisorName,
                                                     bool isNewAdvisor,
                                                     bool isOldAdvisor)
    {
        string body;
        //We have new advisor, old advisor, students
        if (isNewAdvisor&&UserType.FacultyMember==userType)
        {
            body = SocietyAdvisorChangedEmail.NewAdvisorText
                .Replace("{{societyName}}", societyName);
        }
        else if (isOldAdvisor && UserType.FacultyMember == userType)
        {
            body = SocietyAdvisorChangedEmail.OldAdvisorText
                .Replace("{{societyName}}", societyName);
        }
        else if (UserType.Student == userType)
        {
            body = SocietyAdvisorChangedEmail.StudentText
                .Replace("{{userName}}", userName)
                .Replace("{{societyName}}", societyName)
                .Replace("{{oldAdvisorName}}",oldAdvisorName)
                .Replace("{{newAdvisorName}}", newAdvisorName);
        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }
        var subject = societyName + " Important Update";
        await Send(to, subject, body);
    }
    public async Task SendSocietyJoinRequestDecisionMade(string to, string userName, UserType userType, string societyName, bool decision)
    {
        string body;
        if (userType == UserType.Student&&decision)
        {
            body = SocietyJoinRequestEmail.AcceptText
                .Replace("{{societyName}}", societyName)
                .Replace("{{userName}}", userName);
        }
        else if (userType == UserType.Student && !decision)
        {
            body = SocietyJoinRequestEmail.RejectText
                .Replace("{{societyName}}", societyName)
                .Replace("{{userName}}", userName);
        }
        else
        {
            throw new ArgumentException("Invalid user type");
        }
        var subject = "Update On Your Request To Join "+societyName;
        await Send(to, subject, body);
    }
}

