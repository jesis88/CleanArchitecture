using Application.EntityOrder.Commands;
using Domain.Entity;
using Infrastructure.Services.KafkaServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(KafkaProducer producer, KafkaConsumer consumer) : ControllerBase
    {
        private readonly KafkaProducer _producer = producer;
        private readonly KafkaConsumer _consumer = consumer;

        [HttpPost("CreateOrderThroughKafka")]
        public async Task<IActionResult> CreateOrder([FromBody]CreateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderJson = JsonSerializer.Serialize(command.Order);
            await _producer.ProduceAsync("orders", orderJson, cancellationToken);
            return Ok();
        }

        [HttpGet("GetOrderThroughKafka")]
        public IActionResult GetOrder(CancellationToken cancellationToken)
        {
            int limit = 5;
            var messages = _consumer.Consume("orders", limit, cancellationToken).Take(limit);
            var orders = messages.Select(m => JsonSerializer.Deserialize<Order>(m));
            return Ok(orders);
        }
    }
}