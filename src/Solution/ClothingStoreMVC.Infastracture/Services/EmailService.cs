using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace ClothingStoreMVC.Infrastructure.Services
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var settings = _config.GetSection("EmailSettings");

            var client = new SmtpClient(settings["Host"])
            {
                Port = int.Parse(settings["Port"]!),
                Credentials = new NetworkCredential(
                    settings["Username"],
                    settings["Password"]),
                EnableSsl = true,
                UseDefaultCredentials = false 
            };

            var message = new MailMessage
            {
                From = new MailAddress(settings["From"]!, settings["FromName"]),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };
            message.To.Add(to);

            await client.SendMailAsync(message);
        }
    }
}