

using Application.Interfaces;
using Infrastructure.Services;
using Moq;
using System.Net.Mail;

namespace xUnitTesting
{
    public class EmailServiceUT
    {

        private readonly EmailService _emailService;

        public EmailServiceUT()
        {
            _emailService = new EmailService(); 
        }

        [Fact]
        public async Task SendEmailAsync_ShouldNotThrowException_WhenCalledWithValidParameters()
        {
            // Arrange
            string email = "test@test.com";
            string subject = "Test Subject";
            string message = "Test Message";

            // Act
            var ex = await Record.ExceptionAsync(() => _emailService.SendEmailAsync(email, subject, message));

            // Assert
            Assert.Null(ex);
        }

        [Fact]
        public async Task SendEmailAsync_ShouldThrowException_WhenCalledWithInvalidParameters()
        {
            // Arrange
            string email = ""; // Invalid email
            string subject = "Test Subject";
            string message = "Test Message";

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _emailService.SendEmailAsync(email, subject, message));
        }
    }
}
