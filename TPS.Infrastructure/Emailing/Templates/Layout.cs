namespace TPS.Infrastructure.Emailing.Templates;


internal class EmailLayout
{
    public const string Text = @"
    <!DOCTYPE html>
<html>
<head>
  <meta charset=""UTF-8"">
  <title>The Societies Portal</title>
  <style>
   @media only screen and (max-width: 620px) {
      .container {
        width: 100% !important;
        padding: 20px !important;
      }
      .footer-logos {
        flex-direction: column !important;
        gap: 10px !important;
      }
      .footer-logos img {
        height: 36px !important;
      }
    }
  </style>
</head>
<body style=""margin: 0; padding: 0; background-color: #f9fafb; font-family: 'Segoe UI', sans-serif;"">

  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #f9fafb;"">
    <tr>
      <td align=""center"">
        <!-- Container -->
        <table class=""container"" width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color: #ffffff; margin: 40px 0; border-radius: 12px; overflow: hidden; box-shadow: 0 8px 30px rgba(0,0,0,0.05);"">
          
          <!-- Email Body -->
          <tr>
            <td style=""padding: 40px 30px 30px 30px;"">
              <!-- Email Content Starts Here -->
              {{body}}
              <!-- Email Content Ends Here -->
            </td>
          </tr>

          <!-- Divider -->
          <tr>
            <td style=""padding: 0 30px;"">
              <hr style=""border: none; border-top: 1px solid #e2e8f0; margin: 0;"" />
            </td>
          </tr>

         <tr>
          <td style=""padding: 25px 30px; text-align: center;"">
            <!-- Logos stacked and centered -->
            <div style=""margin-bottom: 16px;"">
              <img src=""https://s14.gifyu.com/images/bsObr.png"" alt=""The Societies Portal Logo"" style=""height: 40px; margin: 0 10px;"">
            </div>

            <!-- Portal Title & Subtitle -->
            <p style=""color: #2d3748; font-size: 16px; font-weight: bold; margin: 0;"">The Societies Portal</p>
            <p style=""color: #718096; font-size: 14px; margin: 4px 0 20px;"">Your Gateway to Student Societies</p>

            <!-- Legal Footer -->
            <p style=""font-size: 12px; color: #a0aec0; margin: 0;"">© 2025 The Societies Portal. All rights reserved.</p>
            <p style=""font-size: 12px; color: #a0aec0; margin: 4px 0 0;"">
              <a href=""https://the-societies-portal.web.app/terms-of-use"" style=""color: #718096; text-decoration: underline;"">Terms of use</a> |
              <a href=""https://the-societies-portal.web.app/privacy-policy"" style=""color: #718096; text-decoration: underline;"">Privacy Policy</a>
            </p>
          </td>
        </tr>


        </table>
      </td>
    </tr>
  </table>

</body>
</html>



";
}
