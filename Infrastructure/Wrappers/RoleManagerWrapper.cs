
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Wrappers
{
    public class RoleManagerWrapper : IRoleManagerWrapper
    {
        private readonly RoleManager<IdentityRole<string>> _roleManager;

        public RoleManagerWrapper(RoleManager<IdentityRole<string>> roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<bool> RoleExistsAsync(string roleName)
        {
            return _roleManager.RoleExistsAsync(roleName);
        }
    }
}
