using DotNetEnv;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MimeKit.Text;

namespace SocialMedia_App.Services
{
    public class EmailService : IEmailSender
    {

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Env.Load();
            const string header = @"
                <table width=""100%"" bgcolor=""#f0f0f0"" cellpadding=""0"" cellspacing=""0"">
                  <tr>
                    <td align=""center"">
                      <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#ffffff;"">
                        <tr>
                          <td bgcolor=""#4A90E2"" style=""padding:20px; text-align:center; color:#ffffff; 
                             font-family:Arial,sans-serif; font-size:24px;"">
                            EchoSocial
                          </td>
                        </tr>
                        <tr>
                          <td style=""padding:20px; font-family:Arial,sans-serif;"">
                ";

            const string footer = @"
                      </td>
                    </tr>
                    <tr>
                      <td bgcolor=""#4A90E2"" style=""padding:20px; text-align:center; color:#ffffff; 
                         font-family:Arial,sans-serif; font-size:12px;"">
                        &copy; 2025 EchoSocial
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
            ";
            string mail = Env.GetString("MAIL");
            string password = Env.GetString("PASSWORD");
            using var body = new TextPart(TextFormat.Html);
            body.Text = header + htmlMessage + footer;

            using var message = new MimeMessage();
            message.From.Add(new MailboxAddress(null, "noreply@example.com"));
            message.To.Add(new MailboxAddress(null, email));
            message.Subject = subject;
            message.Body = body;

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            await client.AuthenticateAsync(mail, password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
