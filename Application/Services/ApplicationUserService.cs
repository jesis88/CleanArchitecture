using AutoMapper;
using Domain.Entity;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class ApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<ApplicationUser> CreateUserAsync(User user, string password)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(user);
            var result = await _userManager.CreateAsync(applicationUser, password);

            if (result.Succeeded)
            {
                return _mapper.Map<ApplicationUser>(applicationUser);
            }
            else
            {
                // Handle errors
                throw new Exception("User creation failed");
            }
        }
    }
}
