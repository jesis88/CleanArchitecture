using Application.EntityUser.Commands.CommandHandlers;
using Application.Interfaces;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.EntityUser.Query.QueryHandler
{
    public class RefreshTokenHandler(IUserManagerWrapper userManager, IConfiguration configuration, IRefreshTokenService refreshTokenService) : IRequestHandler<RefreshTokenQuery, string>
    {
        private readonly IUserManagerWrapper _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;

        public async Task<string> Handle(RefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var refreshTokenValue = _refreshTokenService.GetRefreshToken(request.RefreshToken);
            ThrowIfNull(refreshTokenValue, "Invalid Refresh Token");

            var (userName, expiryDate) = refreshTokenValue!.Value;
            if (DateTime.Now > expiryDate)
                throw new InvalidOperationException("Refresh Token has expired");

            var user = await _userManager.FindByNameAsync(userName);
            ThrowIfNull(user, "Invalid User");

            return await _refreshTokenService.GetJWTAndRefreshToken(_userManager, _configuration, userName, user!);
        }

        private static void ThrowIfNull(object? value, string errMessage)
        {
            if(value == null)
            {
                throw new InvalidOperationException(errMessage);
            }
        }

    }
}
