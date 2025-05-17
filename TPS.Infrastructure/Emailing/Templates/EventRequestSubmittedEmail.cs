using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Infrastructure.Emailing.Templates
{
    internal class EventRequestSubmittedEmail
    {
        public const string StudentText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Event Request Submitted ✅</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Hello {{userName}}, your request to organize the event <strong>{{eventName}}</strong> has been submitted successfully.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              The request is currently under review by your advisor and the dean assistant. You will be notified once a decision is made.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              Keep an eye on the portal for status updates and follow up if needed.
            </p>

            <a href=""https://the-societies-portal.web.app/student-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Go to Your Dashboard
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""https://the-societies-portal.web.app/help"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";

        public const string FacultyMemberText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">New Event Request Submitted 📩</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Hello Dr. {{userName}}, a new event request titled <strong>{{eventName}}</strong> has been submitted and is now awaiting consideration.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              You can review all relevant details and provide your decision through the portal.
            </p>

            <a href=""https://the-societies-portal.web.app/admin-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Review Event Request
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""https://the-societies-portal.web.app/help"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";
    }
}
