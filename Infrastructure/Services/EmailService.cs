using Application.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly static string personalEmail = "jesismaharjan88@gmail.com";
        private readonly static string personalPassword = Environment.GetEnvironmentVariable("EMAIL_APP_PASSWORD")!;

        //for testing mails
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(personalEmail, personalPassword),
                    EnableSsl = true,
                };

                await smtpClient.SendMailAsync(personalEmail, email, subject, message);
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException($"There was an error: {ex.Message}");
            }
            
        }
    }
}
