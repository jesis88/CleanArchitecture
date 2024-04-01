using Application.Interfaces;
using AutoMapper;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Commands.CommandHandlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserManagerWrapper _userManager;
        private readonly IRoleManagerWrapper _roleManager;
        private readonly IMapper _mapper;

        public CreateUserHandler(IUserManagerWrapper userManager, IRoleManagerWrapper roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(command);

            var result = await _userManager.CreateAsync(user, command.Password);
            if (result.Succeeded)
            {
                if(Enum.TryParse<Role>(command.Role.ToString(), out var roleEnum) && await _roleManager.RoleExistsAsync(roleEnum.ToString()))
                {
                    var roles = new List<string> { roleEnum.ToString() };
                    await _userManager.AddToRolesAsync(user, roles);
                }
            }
            return "User created successfully";
        }
    }
}
