namespace Application.Interfaces
{
    public interface IEmailService
    {
        void SendEmailAsync(string email, string subject, string message);
    }
}
