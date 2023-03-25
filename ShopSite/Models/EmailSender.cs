using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace ShopSite.Models
{
    public class EmailSender : IEmailSender
    {
        private readonly IOptions<SMTPSettings> _smtpSettings;
        public EmailSender(IOptions<SMTPSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task SendAsync(string senderName, string to, string subject, string body)
        {
            var message = new MailMessage(_smtpSettings.Value.User, to, subject, body);
            message.From = new MailAddress(_smtpSettings.Value.User, senderName);

            using (var smtp = new SmtpClient(_smtpSettings.Value.Host, _smtpSettings.Value.Port))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(
                    _smtpSettings.Value.User, _smtpSettings.Value.Password);
                await smtp.SendMailAsync(message);
            }
        }
    }
}
