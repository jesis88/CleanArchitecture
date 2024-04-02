using Application.Interfaces;
using System.Collections.Concurrent;

namespace Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ConcurrentDictionary<string, (string UserName, DateTime ExpiryDate)> _refreshTokens = new ConcurrentDictionary<string, (string, DateTime)>();

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
    }
}
