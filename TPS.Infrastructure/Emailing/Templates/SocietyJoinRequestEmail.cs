using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPS.Infrastructure.Emailing.Templates
{
    internal class SocietyJoinRequestEmail
    {
        public const string AcceptText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #2f855a;"">You're In! Welcome to <strong>{{societyName}}</strong> 🌱</h2>

            <p style=""font-size: 16px; color: #2d3748; margin: 0 0 20px;"">
                Hello {{userName}}, your request to join <strong>{{societyName}}</strong> has been warmly accepted.
            </p>

            <p style=""font-size: 16px; color: #2d3748; margin: 0 0 20px;"">
                Thank you for stepping forward to be part of something meaningful. Your willingness to give time and energy to serve others makes a real difference.
            </p>

            <p style=""font-size: 16px; color: #2d3748;"">
                We’re excited to see the impact you’ll make. Visit the portal to connect with the community and get started on your volunteering journey.
            </p>
        ";

        public const string RejectText = @"
            <h2 style=""margin: 0 0 16px; font-size: 24px; color: #c53030;"">Request Review Outcome – <strong>{{societyName}}</strong></h2>

            <p style=""font-size: 16px; color: #2d3748; margin: 0 0 20px;"">
                Hello {{userName}}, thank you for your interest in volunteering with <strong>{{societyName}}</strong>. While your request wasn’t approved this time, we truly appreciate your spirit and initiative.
            </p>

            <p style=""font-size: 16px; color: #2d3748; margin: 0 0 20px;"">
                There are always new chances to make a difference. We encourage you to stay involved, explore other societies, and keep sharing your energy and passion for helping others.
            </p>

            <p style=""font-size: 16px; color: #2d3748;"">
                Every act of kindness matters — and yours will find its place.
            </p>
        ";
    }
}
