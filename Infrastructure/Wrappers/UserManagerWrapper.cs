using Application.Interfaces;
using AutoMapper;
using Domain.Entity;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;

namespace CQRSApplication.UserManagerWrappers
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

        public Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string>role)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(user);
            return _userManager.AddToRolesAsync(applicationUser, role);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(user);
            return _userManager.GetRolesAsync(applicationUser);
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

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(user);
            return _userManager.CheckPasswordAsync(applicationUser, password);
        }

        public Task<IdentityResult> UpdateAsync(User user)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(user);
            return _userManager.UpdateAsync(applicationUser);
        }
    }
}
