using Application.EntityUser.Commands.CommandHandlers;
using Application.Interfaces;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ConcurrentDictionary<string, (string UserName, DateTime ExpiryDate)> _refreshTokens = new();

        public void AddRefreshToken(string refreshToken, string userName, DateTime expiryDate)
        {
            _refreshTokens.TryAdd(refreshToken, (userName, expiryDate));
        }

        public (string UserName, DateTime ExpiryDate)? GetRefreshToken(string refreshToken)
        {
            if (_refreshTokens.TryGetValue(refreshToken, out var refreshTokenValue))
            {
                return refreshTokenValue;
            }
            return null;
        }

        public string GetUserName()
        {
            return _refreshTokens.First().Value.UserName;
        }

        public async Task<string> GetJWTAndRefreshToken(IUserManagerWrapper userManager, IConfiguration configuration, string userName, User user)
        {
            var jwtKey = configuration["Jwt:Key"];
            if(jwtKey != null) 
            {
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var roles = await userManager.GetRolesAsync(user);
                var claims = new List<Claim>();
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Name, userName));
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var tokenOptions = new JwtSecurityToken(
                    issuer: configuration["Jwt:Issuer"],
                    audience: configuration["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(60),
                    signingCredentials: signinCredentials
                );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                var refreshToken = GenerateRefreshToken();
                var tokenData = new
                {
                    AccessToken = jwtToken,
                    RefreshToken = refreshToken.Token
                };

                AddRefreshToken(refreshToken.Token, userName, refreshToken.Expires);
                return JsonConvert.SerializeObject(tokenData);
            }
            else
            {
                throw new InvalidOperationException("Jwt key is null");
            }
        }

        private static RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddMinutes(1)
            };
        }
    }
}
