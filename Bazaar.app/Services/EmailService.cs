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

            string htmlBody = $@"
        <div style='font-family: ""Segoe UI"", Tahoma, Geneva, Verdana, sans-serif; max-width: 500px; margin: 0 auto; border: 1px solid #eee; border-radius: 10px; overflow: hidden; box-shadow: 0 4px 10px rgba(0,0,0,0.05);'>
            <div style='background-color: #1a1a1a; padding: 20px; text-align: center;'>
                <h1 style='color: #d4af37; margin: 0; letter-spacing: 2px; text-transform: uppercase;'>BAZAAR</h1>
                <p style='color: #888; margin: 5px 0 0 0; font-size: 12px;'>سوقك لبيع وشراء السيارات</p>
            </div>
            <div style='padding: 30px; background-color: #ffffff; text-align: center; direction: rtl;'>
                <h2 style='color: #333; margin-bottom: 10px;'>رمز التحقق الخاص بك</h2>
                <p style='color: #666; font-size: 14px; margin-bottom: 25px;'>يرجى استخدام الرمز التالي لإتمام عملية تسجيل الدخول. هذا الرمز صالح لمدة 10 دقائق فقط.</p>
                
                <div style='background-color: #f9f9f9; border: 2px dashed #d4af37; padding: 15px; border-radius: 8px; display: inline-block;'>
                    <span style='font-size: 32px; font-weight: bold; color: #1a1a1a; letter-spacing: 5px;'>{otp}</span>
                </div>
                
                <p style='color: #999; font-size: 12px; margin-top: 25px;'>إذا لم تطلب هذا الرمز، يمكنك تجاهل هذا الإيميل بأمان.</p>
            </div>
            <div style='background-color: #f4f4f4; padding: 15px; text-align: center; border-top: 1px solid #eee;'>
                <p style='color: #888; font-size: 11px; margin: 0;'>© {DateTime.Now.Year} Bazaar Team. All rights reserved.</p>
            </div>
        </div>";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username??"bazaar963@gmail.com", "Bazaar - بازار"),
                Subject = $"{otp} هو رمز التحقق لبازار",
                Body = htmlBody,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
