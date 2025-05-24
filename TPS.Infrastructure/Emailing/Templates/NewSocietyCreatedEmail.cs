namespace TPS.Infrastructure.Emailing.Templates
{
    internal class NewSocietyCreatedEmail
    {
        public const string StudentText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Hello, {{userName}}! 👋</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              A new society, <strong>{{societyName}}</strong>, has just been created in the portal!
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              Visit the portal now to learn more about their mission, upcoming events, and how you can be part of their journey.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              💡 Keep the spirit of volunteering alive — consider joining <strong>{{societyName}}</strong> and contribute to something impactful.
            </p>

            <a href=""https://the-societies-portal.web.app/student-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              Explore the New Society
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              Have questions or need help? Reply to this email or visit our <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";

        public const string FacultyMemberText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Greetings, Dr. {{userName}}! 👋</h2>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              A new society named <strong>{{societyName}}</strong> has been officially created in the portal!
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
              We invite you to visit the portal to learn more about this new initiative and the students behind it.
            </p>

            <p style=""font-size: 16px; color: #4a5568; margin: 0 0 24px;"">
              🎓 Your guidance and support are essential to helping new societies grow and succeed. We encourage you to engage with <strong>{{societyName}}</strong> and offer your mentorship.
            </p>

            <a href=""https://the-societies-portal.web.app/admin-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
              View New Society
            </a>

            <p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
              Need assistance? Just reply to this email or check our <a href=""href=""href=""https://the-societies-portal.web.app/help-center"" style=""color: #3182ce;"">Help Center</a>.
            </p>
            ";
    }
}
