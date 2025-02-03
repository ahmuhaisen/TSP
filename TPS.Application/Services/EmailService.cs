using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using TPS.Application.Abstractions;
using TSP.Domain.Shared.Options;

namespace TPS.Application.Services;

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

        var emailBody = emailTemplate
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

    private readonly string emailTemplate = @"
      <!DOCTYPE html>
<html>
<head>
    <style>
        body { 
            font-family: Arial, sans-serif; 
            background-color: #f4f4f4; 
            margin: 0; 
            padding: 0; 
        }
        .email-container { 
            max-width: 600px; 
            margin: 20px auto; 
            padding: 0; 
            background: #ffffff;
            border: 1px solid #dddddd;
            border-radius: 8px;
            overflow: hidden;
        }
        .email-header { 
            width: 100%;
            background-color: #007bff; 
            color: #ffffff; 
            text-align: center; 
            padding: 20px; 
            font-size: 24px;
            display: flex;
            justify-content: center;
            align-items: center;
            gap: 10px;
        }
        .email-header img { 
            width: 55px; 
            height: auto; 
            margin-bottom: 10px; 
        }
        .email-body { 
            width: 100%; 
            min-height: calc(100vh - 120px); /* Subtract header and footer height */
            padding: 20px; 
            font-size: 16px; 
            line-height: 1.5; 
            color: #333333; 
            box-sizing: border-box; 
        }
        .email-footer { 
            background-color: #f8f9fa; 
            text-align: center; 
            padding: 10px; 
            font-size: 12px; 
            color: #999999; 
        }
    </style>
</head>
<body>
    <div class=""email-container"">
        <div class=""email-header"">
            <img src=""https://i.ibb.co/N6RcYQB/tsp-logo.png"" alt=""The Societies Portal Logo"">
            <span>The Societies Portal</span>
        </div>
        <div class=""email-body"">
            {{body}}
        </div>
        <div class=""email-footer"">
            &copy; {{year}} The Societies Portal. All rights reserved.
        </div>
    </div>
</body>
</html>
";
}

