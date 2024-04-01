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
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> UpdateAsync(User user);
    }
}
