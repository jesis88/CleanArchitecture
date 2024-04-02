using Application.Interfaces;
using AutoMapper;
using Domain.Entity;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Wrappers
{
    public class UserManagerWrapper : IUserManagerWrapper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UserManagerWrapper(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string>role)
        {
            var applicationUser = await _userManager.FindByNameAsync(user.UserName);
            return await _userManager.AddToRolesAsync(applicationUser, role);
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            var applicationUser = await _userManager.FindByNameAsync(user.UserName);
            return await _userManager.GetRolesAsync(applicationUser);
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(user);
            return _userManager.CreateAsync(applicationUser, password);
        }

        public async Task<User?> FindByNameAsync(string userName)
        {
            var applicationUser = await _userManager.FindByNameAsync(userName);
            if (applicationUser == null)
            {
                return null;
            }

            var user = _mapper.Map<User>(applicationUser);
            return user;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var applicationUser = await _userManager.FindByNameAsync(user.UserName);
            return await _userManager.CheckPasswordAsync(applicationUser, password);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            var applicationUser = await _userManager.FindByNameAsync(user.UserName);
            return await _userManager.UpdateAsync(applicationUser);
        }
    }
}
