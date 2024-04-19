using Confluent.Kafka;
namespace Infrastructure.Services.KafkaServices
{
    public class KafkaProducer
    {
        private readonly IProducer<Null, string> _producer;

        public KafkaProducer()
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }

        public async Task ProduceAsync(string topic, string message, CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message }, cancellationToken);
        }
    }
}
