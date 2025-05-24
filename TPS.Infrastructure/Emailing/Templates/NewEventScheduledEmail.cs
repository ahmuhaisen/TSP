using System;

namespace TPS.Infrastructure.Emailing.Templates
{
    internal class NewEventScheduledEmail
    {
        public const string StudentText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">New Event Just Announced 🎉</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Hello {{userName}}, the event <strong>{{eventName}}</strong> has been published and is now open for participation.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Details, schedule, and participation options are available on the portal. Take this opportunity to engage, learn, and connect through a meaningful activity.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              Consider attending to continue your volunteering journey and make the most of your time with the community.
            </p>

            <a href=""https://the-societies-portal.web.app/student-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              View Event Details
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";

        public const string FacultyMemberText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">New Event Scheduled 📅</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Hello Dr. {{userName}}, the student-led event <strong>{{eventName}}</strong> has been published and added to the upcoming schedule.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Full event details and its purpose can be explored through the portal. Faculty engagement is a valued part of the student experience.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              Consider attending or supporting the event to guide and encourage participants.
            </p>

            <a href=""https://the-societies-portal.web.app/admin-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              View Event Details
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";
    }
}
