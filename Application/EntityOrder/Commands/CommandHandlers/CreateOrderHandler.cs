using Application.Interfaces;
using Domain.Entity;
using MediatR;

namespace Application.EntityOrder.Commands.CommandHandlers
{
    public class CreateOrderHandler(IOrderWrapper orderWrapper) : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IOrderWrapper _orderWrapper = orderWrapper;

        public async Task<int> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var status = await _orderWrapper.AddOrder(command.Order!);
            return status;
        }
    }
}