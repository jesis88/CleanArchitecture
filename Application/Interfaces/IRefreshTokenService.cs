namespace Application.Interfaces
{
    public interface IRefreshTokenService
    {
        void AddRefreshToken(string refreshToken, string userName, DateTime expiryDate);
        (string UserName, DateTime ExpiryDate)? GetRefreshToken(string refreshToken);
        string GetUserName();
    }
}
