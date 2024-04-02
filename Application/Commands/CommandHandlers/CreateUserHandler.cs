using Application.Interfaces;
using AutoMapper;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace Application.Commands.CommandHandlers
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IUserManagerWrapper _userManager;
        private readonly IRoleManagerWrapper _roleManager;

        public CreateUserHandler(IUserManagerWrapper userManager, IRoleManagerWrapper roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = new User() 
            { 
                Id = Guid.NewGuid().ToString(),
                UserName = command.UserName,
                Email = command.Email
            };

            var result = await _userManager.CreateAsync(user, command.Password);
            if (result.Succeeded)
            {
                var textInfo = new CultureInfo("en-US", false).TextInfo;
                var role = textInfo.ToTitleCase(command.Role.ToString().ToLower());
                if (Enum.TryParse<Role>(role, out var roleEnum) && await _roleManager.RoleExistsAsync(roleEnum.ToString()))
                {
                    var roles = new List<string> { roleEnum.ToString() };
                    await _userManager.AddToRolesAsync(user, roles);
                }
                else
                {
                    return $"User with role {roleEnum} does not exist. Please enter any of these roles {string.Join(",", Enum.GetNames<Role>())}";
                }
            }
            return "User created successfully";
        }
    }
}
