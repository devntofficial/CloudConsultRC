using Confluent.Kafka;

namespace CloudConsult.Common.Kafka
{
    public class KafkaProducer<TValue> : IDisposable, IKafkaProducer<TValue> where TValue : class
    {
        private readonly IProducer<Null, TValue> producer;

        public KafkaProducer(ProducerConfig config)
        {
            producer = new ProducerBuilder<Null, TValue>(config).SetValueSerializer(new KafkaSerializer<TValue>()).Build();
        }

        public async Task<DeliveryResult<Null, TValue>> ProduceAsync(string topic, TValue value, CancellationToken token)
        {
            return await producer.ProduceAsync(topic, new Message<Null, TValue> { Value = value }, token);
        }

        public void Dispose()
        {
            producer.Flush();
            producer.Dispose();
        }
    }
}
