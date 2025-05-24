namespace TPS.Infrastructure.Emailing.Templates
{
    internal class CommiteeChangeEmail
    {
        public const string StudentText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Hello, {{userName}}! 👋</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Changes have been made to <strong>{{societyName}}</strong> committee!
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              <strong>{{committeeName}}</strong> has just been promoted to a committee member within the society. Stay tuned for upcoming activities and initiatives led by the new committee.
            </p>

            <a href=""https://the-societies-portal.web.app/student-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Go to Your Dashboard
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              If you have any questions or need help, just reply to this email or visit our <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";

        public const string FacultyMemberText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Greetings, Dr. {{userName}}! 👋</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Changes have been made to the society you advise '<strong>{{societyName}}</strong>' related to committee!
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              <strong>{{committeeName}}</strong> has been promoted to a committee member. We look forward to your continued support in guiding and mentoring the student leaders of this society.
            </p>

            <a href=""https://the-societies-portal.web.app/admin-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Go to Your Dashboard
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              If you have any questions or need help, just reply to this email or visit our <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";

        public const string SelfUserText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Congratulations, {{userName}}! 🎉</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              We’re thrilled to announce that you have been promoted to a committee member in <strong>{{societyName}}</strong> society. Your leadership and commitment will play a key role in shaping your society’s journey.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Make the most of your new role:
            </p>

            <ul style=""padding-left: 20px; color: #4a5568; font-size: 16px; margin: 0 0 24px;"">
              <li>📌 Manage society events and updates</li>
              <li>🗂️ Collaborate with fellow committee members</li>
              <li>📈 Inspire and lead student engagement</li>
            </ul>

            <a href=""https://the-societies-portal.web.app/student-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Go to Your Dashboard
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              If you have any questions or need help, just reply to this email or visit our <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";
    }
}