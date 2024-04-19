using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;
using Hangfire;
using FluentValidation;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Application.EntityUser.Commands;

namespace Application.EntityUser.Commands.CommandHandlers
{
    public class LoginHandler(IUserManagerWrapper userManager, IConfiguration configuration, IRefreshTokenService refreshTokenService, IBackgroundJobClient backgroundJobClient) : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserManagerWrapper _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;

        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(command.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, command.Password))
            {
                //store recent logged in date (for welcome mail)
                if (!user.RecentLogin.HasValue || user.RecentLogin.Value.AddMinutes(1) <= DateTime.Now)
                {
                    _backgroundJobClient.Enqueue<IEmailService>(emailService => emailService.SendEmailAsync("bravometal88@gmail.com", "Hello", "This is my email to you!"));

                    user.RecentLogin = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                }

                return await _refreshTokenService.GetJWTAndRefreshToken(_userManager, _configuration, command.Username, user);
            }

            throw new InvalidOperationException("Incorrect details for login");
        }
    }

    public class RefreshToken
    {
        public required string Token { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}
