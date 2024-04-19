using MediatR;

namespace Application.EntityUser.Query
{
    public class RefreshTokenQuery : IRequest<string>
    {
        public required string RefreshToken { get; set; }
    }
}
