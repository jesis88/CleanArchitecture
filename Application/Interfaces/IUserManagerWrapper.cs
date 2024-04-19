using Application.EntityUser.Query;
using Domain.Entity;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IUserManagerWrapper
    {
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> role);
        Task<IList<string>> GetRolesAsync(User user);
        Task<User?> FindByNameAsync(string userName);
        Task<User?> FindByEmailAsync(string email);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
        Task<List<User>> ToListAsync(GetUserListQuery query, CancellationToken cancellationToken);
    }
}
