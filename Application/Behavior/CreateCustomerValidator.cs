using Application.EntityCustomer.Commands;
using FluentValidation;

namespace Application.Behavior
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Customer name is required")
                .NotEmpty()
                .WithMessage("Customer name is required");

            RuleFor(x => x.Address)
                .NotNull()
                .WithMessage("Customer address is required")
                .NotEmpty()
                .WithMessage("Customer address is required");

            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .WithMessage("Customer phonenumber is required")
                .NotEmpty()
                .WithMessage("Customer phonenumber is required");

            RuleFor(x => x.UserName)
                .NotNull()
                .WithMessage("UserName is required")
                .NotEmpty()
                .WithMessage("UserName is required");
        }

    }
}
