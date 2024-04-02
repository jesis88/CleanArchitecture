using Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly static string personalEmail = "jesismaharjan88@gmail.com";
        private readonly static string personalPassword = "ytgi tqfs qlen kmdj";

        //for testing mails
        public async void SendEmailAsync(string receiverEmail, string subject, string message)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(personalEmail, personalPassword),
                EnableSsl = true,
            };

            await smtpClient.SendMailAsync(personalEmail, receiverEmail, subject, message);
        }
    }
}
