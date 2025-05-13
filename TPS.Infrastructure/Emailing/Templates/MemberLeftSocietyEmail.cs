using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Infrastructure.Emailing.Templates
{
    internal class MemberLeftSocietyEmail
    {
        public const string UserText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Member Update Notice</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              This is to inform you that <strong>{{userNameLeft}}</strong> is no longer a member of the <strong>{{societyName}}</strong> society.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              The change has been reflected in the system, and the member is no longer associated with the society.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              For details or further updates, please visit the portal.
            </p>

            <a href=""https://the-societies-portal.web.app"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Go to Portal
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""https://the-societies-portal.web.app/help"" style=""color: #3182ce;"">Help Center</a>.
            </p>
        ";
    }
}
