using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Infrastructure.Emailing.Templates;

internal class ResetPasswordEmail
{
    public const string ResetMessage = @"
  <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Reset Your Password 🛡️</h2>

  <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
    Hello {{userName}}, we received a request to reset the password for your account on the Societies Portal.
  </p>

  <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
    If you initiated this request, please click the button below to set a new password. This link is valid for a limited time.
  </p>

  <a href=""{{resetLink}}"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
    Reset Password
  </a>

  <p style=""font-size: 16px; color: #4a5568; margin: 24px 0;"">
    If you did not request a password reset, please ignore this email. Your account remains secure.
  </p>
        ";
}
