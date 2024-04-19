using Application.EntityUser.Commands;
using Application.Interfaces;
using FluentValidation;

namespace Application.Behavior
{
    public class UserRegisterValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserManagerWrapper _userManagerWrapper;
        public UserRegisterValidator(IUserManagerWrapper userManagerWrapper)
        {
            _userManagerWrapper = userManagerWrapper;
            RuleFor(user => user.UserName)
                .NotNull()
                .WithMessage("Username is required.")
                .Length(4, 50)
                .WithMessage("Username must be between 4 to 50 characters.");

            RuleFor(user => user.Email)
                .NotNull()
                .WithMessage("Email is required.")
                .Length(4, 80)
                .WithMessage("Email must be between 4 to 80 characters.")
                .Must(email => !EmailAlreadyExists(email))
                .WithMessage("The email already exist.");

            RuleFor(user => user.Password)
                .NotNull()
                .WithMessage("Password is required.")
                .Length(8, 100)
                .WithMessage("Password must be between 8 to 100 characters.");

            RuleFor(user => user.ConfirmPassword)
                .NotNull()
                .WithMessage("Confirm Password is required.")
                .Length(8, 100)
                .WithMessage("Confirm Password must be between 8 to 100 characters.")
                .Equal(user => user.Password)
                .WithMessage("Passwords must match!");

            RuleFor(user => user.Role)
                .NotNull()
                .WithMessage("Role must be set")
                .IsInEnum()
                .WithMessage("Role is not valid");
        }
        public bool EmailAlreadyExists(string email)
        {
            var result = _userManagerWrapper.FindByEmailAsync(email).Result;
            return result == null;
        }
    }
}
