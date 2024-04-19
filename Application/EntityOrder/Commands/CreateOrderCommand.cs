using Domain.Entity;
using MediatR;

namespace Application.EntityOrder.Commands
{
    public class CreateOrderCommand : IRequest<int>
    {
        public Order? Order { get; set; }
    }
}