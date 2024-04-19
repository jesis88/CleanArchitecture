using Confluent.Kafka;

namespace Infrastructure.Services.KafkaServices
{
    public class KafkaConsumer
    {
        private readonly IConsumer<Ignore, string> _consumer;

        public KafkaConsumer()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "group111",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        }

        public IEnumerable<string> Consume(string topic, int maxMessages, CancellationToken cancellationToken)
        {
            _consumer.Subscribe(topic);

            int messageCount = 0;
            while (!cancellationToken.IsCancellationRequested && messageCount < maxMessages)
            {
                var consumeResult = _consumer.Consume(TimeSpan.FromSeconds(5)); // Set a timeout of 5 seconds
                if (consumeResult != null)
                {
                    yield return consumeResult.Message.Value;
                    messageCount++;
                }
                else
                {
                    break; // Break the loop if no message is received within the timeout
                }
            }
        }
    }
}
