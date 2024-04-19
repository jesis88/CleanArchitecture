using Application.Interfaces;
using Domain.Entity;
using FluentValidation;
using MediatR;

namespace Application.EntityCustomer.Commands.CommandHandlers
{
    public class CreateCustomerHandler(ICustomerWrapper customerDbWrapper, IUserManagerWrapper userManager) : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly ICustomerWrapper _customerDbWrapper = customerDbWrapper;
        private readonly IUserManagerWrapper _userManager = userManager;

        public async Task<int> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var user  = await _userManager.FindByNameAsync(command.UserName);
            var newCustomer = new Customer
            {
                Name = command.Name,
                Address = command.Address,
                PhoneNumber = command.PhoneNumber,
                UserId = user!.Id
            };

            return await _customerDbWrapper.AddCustomer(newCustomer);
        }
    }
}
