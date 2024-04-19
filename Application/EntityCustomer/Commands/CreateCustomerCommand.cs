using MediatR;

namespace Application.EntityCustomer.Commands
{
    public class CreateCustomerCommand : IRequest<int>
    {
        public string? Name { get; set; }
        public required string UserName { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public bool Active { get; set; } = true;
    }
}
