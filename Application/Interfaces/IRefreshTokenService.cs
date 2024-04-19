using Domain.Entity;
using Microsoft.Extensions.Configuration;

namespace Application.Interfaces
{
    public interface IRefreshTokenService
    {
        void AddRefreshToken(string refreshToken, string userName, DateTime expiryDate);
        (string UserName, DateTime ExpiryDate)? GetRefreshToken(string refreshToken);
        string GetUserName();
        Task<string> GetJWTAndRefreshToken(IUserManagerWrapper userManager, IConfiguration configuration, string userName, User user);
    }
}
