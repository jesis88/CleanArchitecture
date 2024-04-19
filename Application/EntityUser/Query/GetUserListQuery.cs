using Domain.Entity;
using MediatR;

namespace Application.EntityUser.Query
{
    public class GetUserListQuery : IRequest<List<User>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }
}