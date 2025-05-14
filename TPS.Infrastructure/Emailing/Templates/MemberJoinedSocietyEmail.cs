using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Infrastructure.Emailing.Templates
{
    internal class MemberJoinedSocietyEmail
    {
        public const string UserText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Welcome a New Member 🎉</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Great news! <strong>{{userNameJoined}}</strong> has officially joined the <strong>{{societyName}}</strong> society.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              A warm welcome to the new member — it’s always exciting to see the community grow with passionate individuals ready to contribute and collaborate.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              The member list has been updated accordingly. Feel free to reach out, share ideas, and make them feel at home within the society.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              Let’s continue building a vibrant and supportive environment together.
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
