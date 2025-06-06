using Microsoft.AspNetCore.Identity.UI.Services;

namespace SocialMedia_App.Services
{
    public class EmailService : IEmailSender
    {
        /// <summary>
        /// A “no‐op” IEmailSender: it implements SendEmailAsync but does nothing.
        /// During development you can keep this, then swap in a real SMTP/SENDGRID
        /// version later.
        /// </summary>
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
