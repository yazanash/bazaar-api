using Bazaar.app.Helpers;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Bazaar.app.Services
{
    public class EmailService
    {
        private readonly SmtpSettings _smtpSettings;
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }
        public async Task SendEmailAsync(string email, int otp)
        {
            var smtpClient = new SmtpClient(_smtpSettings.Server)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true,
            };
            string htmlBody = $@"<h1> your ot is {otp} </h1>";
            var mailMessage = new MailMessage
            {
                From = new MailAddress("bazaar@gmail.com"),
                Subject = "Your OTP Code",
                Body = htmlBody,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
