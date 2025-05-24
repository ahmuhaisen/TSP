using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Infrastructure.Emailing.Templates
{
    internal class SocietyAdvisorChangedEmail
    {
        public const string StudentText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Update in Your Society's Advisory Team 🔄</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Hello {{userName}}, this is to inform you that <strong>Dr. {{oldAdvisorName}}</strong> will no longer be the advisor for the <strong>{{societyName}}</strong> society.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              <strong>Dr. {{newAdvisorName}}</strong> has taken on the advisory role for the society. You can now refer to them for guidance, support, and future coordination.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              Stay tuned on the portal for updates and announcements, and continue contributing positively to your society’s journey.
            </p>

            <a href=""https://the-societies-portal.web.app/student-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Visit the Portal
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
        ";

        public const string OldAdvisorText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Thank You for Your Dedication 🙏</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Your time supporting <strong>{{societyName}}</strong> as an advisor has come to an end, leaving a lasting impact through your guidance and dedication. Thank you for the time, effort, and care you’ve given to help guide the students and support their initiatives.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Your contributions have left a meaningful impact and are genuinely appreciated by everyone involved.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              Wishing you continued success and fulfillment in your future endeavors.
            </p>

            <a href=""https://the-societies-portal.web.app/admin-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Visit the Portal
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
        ";

        public const string NewAdvisorText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Welcome to Your New Role 👋</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              You have been assigned as the advisor for the <strong>{{societyName}}</strong> society. Your guidance and support will be instrumental in shaping the journey of its members.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              This new role opens doors for inspiring young minds, fostering collaboration, and helping initiatives thrive under your leadership.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              Wishing you great achievements and impactful moments with the team ahead.
            </p>

            <a href=""https://the-societies-portal.web.app/admin-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Visit the Portal
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              For assistance or questions, reply to this email or visit the <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
        ";
    }
}
