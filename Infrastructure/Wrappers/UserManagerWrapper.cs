using Application.EntityUser.Query;
using Application.Interfaces;
using AutoMapper;
using Domain.Entity;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Wrappers
{
    public class UserManagerWrapper(UserManager<ApplicationUser> userManager, IMapper mapper) : IUserManagerWrapper
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;

        public async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string>role)
        {
            ThrowIfNull(user.UserName,"Username is null");
            var applicationUser = await _userManager.FindByNameAsync(user.UserName!);
            ThrowIfNull(applicationUser, "ApplicationUser is null");

            return await _userManager.AddToRolesAsync(applicationUser!, role);
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            var applicationUser = await _userManager.FindByNameAsync(user.UserName!);
            return await _userManager.GetRolesAsync(applicationUser!);
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(user);
            return _userManager.CreateAsync(applicationUser, password);
        }

        public async Task<User?> FindByNameAsync(string userName)
        {
            var applicationUser = await _userManager.FindByNameAsync(userName);
            ThrowIfNull(applicationUser, "ApplicationUser is null");
            var user = _mapper.Map<User>(applicationUser);
            return user;
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            var applicationUser = await _userManager.FindByEmailAsync(email);
            ThrowIfNull(applicationUser,"ApplicationUser is null");
            var user = _mapper.Map<User>(applicationUser);
            return user;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            var applicationUser = await _userManager.FindByNameAsync(user.UserName!);
            return await _userManager.CheckPasswordAsync(applicationUser!, password);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            var applicationUser = await _userManager.FindByNameAsync(user.UserName!);
            applicationUser!.RecentLogin = user.RecentLogin;
            return await _userManager.UpdateAsync(applicationUser);
        }

        public async Task<List<User>> ToListAsync(GetUserListQuery query, CancellationToken cancellationToken)
        {
            var applicationUser = await _userManager.Users.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync(cancellationToken);
            return _mapper.Map<List<User>>(applicationUser);
        }

        private static void ThrowIfNull(object? value, string errorMessage)
        {
            if(value == null)
                throw new InvalidOperationException(errorMessage);
        }
    }
}
