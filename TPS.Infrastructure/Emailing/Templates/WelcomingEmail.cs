namespace TPS.Infrastructure.Emailing.Templates;
internal class WelcomingEmail
{
    public const string StudentText = @"
   <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Welcome, {{userName}}! 👋</h2>

<p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
  We’re thrilled to have you on board at <strong>The Societies Portal</strong>. Whether you’re looking to join exciting student societies, participate in events, or collaborate with like-minded peers — you’re in the right place.
</p>

<p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
  Here’s how you can get started:
</p>

<ul style=""padding-left: 20px; color: #4a5568; font-size: 16px; margin: 0 0 24px;"">
  <li>🎓 Explore societies that match your interests</li>
  <li>📅 Stay updated with upcoming events</li>
  <li>🤝 Connect with fellow students and collaborators</li>
</ul>

<a href=""https://the-societies-portal.web.app/student-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
  Go to Your Dashboard
</a>

<p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
  If you have any questions or need help, just reply to this email or visit our <a href=""https://the-societies-portal.web.app/help"" style=""color: #3182ce;"">Help Center</a>.
</p>
";

    public const string FacultyMemberText = @"
<h2 style=""margin: 0 0 16px; font-size: 24px; color: #2d3748;"">Welcome, Dr. {{userName}}! 👋</h2>

<p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
  We’re honored to welcome you to <strong>The Societies Portal</strong>. Your role is essential in supporting vibrant student communities, and this platform is designed to help you guide, advise, and oversee student-led initiatives with ease.
</p>

<p style=""font-size: 16px; color: #4a5568; margin: 0 0 20px;"">
  Here’s what you can do:
</p>

<ul style=""padding-left: 20px; color: #4a5568; font-size: 16px; margin: 0 0 24px;"">
  <li>📂 Oversee society proposals and approvals</li>
  <li>📝 Offer guidance and monitor activity reports</li>
  <li>📢 Encourage student engagement through academic support</li>
</ul>

<a href=""https://the-societies-portal.web.app/admin-area"" style=""display: inline-block; background-color: #3182ce; color: #ffffff; text-decoration: none; padding: 12px 24px; font-size: 16px; border-radius: 6px;"">
  Go to Your Dashboard
</a>

<p style=""font-size: 14px; color: #a0aec0; margin-top: 30px;"">
  If you have any questions or need help, just reply to this email or visit our <a href=""https://the-societies-portal.web.app/help"" style=""color: #3182ce;"">Help Center</a>.
</p>
";
}
