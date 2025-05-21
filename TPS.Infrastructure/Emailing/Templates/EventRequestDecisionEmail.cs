using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Infrastructure.Emailing.Templates
{
    internal class EventRequestDecisionEmail
    {
        public const string AcceptText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Event Request Approved 🎉</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Hello {{userName}}, your event request for <strong>{{eventName}}</strong> has been approved.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              The event is now ready to move forward. Head over to your dashboard to start preparing and managing it.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              Wishing you a successful and impactful event.
            </p>

            <a href=""https://the-societies-portal.web.app/student-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Go to Dashboard
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""https://the-societies-portal.web.app/help"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";

        public const string RejectText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Event Request Not Approved</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Hello {{userName}}, the event request for <strong>{{eventName}}</strong> has not been approved.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              <strong>Remarks:</strong> {{remark}}
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              Consider reviewing and adjusting your proposal. You can submit another request after considering the remarks.
            </p>

            <a href=""https://the-societies-portal.web.app/student-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Review
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""https://the-societies-portal.web.app/help"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";
    }
}
