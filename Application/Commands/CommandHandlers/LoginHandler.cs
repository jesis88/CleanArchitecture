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

namespace Application.Commands.CommandHandlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserManagerWrapper _userManager;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenService _refreshTokenService;

        public LoginHandler(IUserManagerWrapper userManager, IConfiguration configuration, IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(command.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, command.Password))
            {
                //store recent logged in date (for welcome mail)
                if (!user.RecentLogin.HasValue || user.RecentLogin.Value.AddMinutes(1) <= DateTime.Now)
                {
                    BackgroundJob.Enqueue<IEmailService>(emailService => emailService.SendEmailAsync("bravometal88@gmail.com", "Hello", "This is my email to you!"));

                    user.RecentLogin = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                }

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>();
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Name, command.Username));
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var tokenOptions = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(30),
                    signingCredentials: signinCredentials
                );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                var refreshToken = GenerateRefreshToken();
                var tokenData = new
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken.Token
                };

                
                _refreshTokenService.AddRefreshToken(refreshToken.Token, command.Username, refreshToken.Expires);
                return JsonConvert.SerializeObject(tokenData);
            }

            throw new Exception("Incorrect details for login");
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMinutes(1)
            };
        }
    }

    public class RefreshToken
    {
        public required string Token { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime Expires { get; set; }
    }
}
