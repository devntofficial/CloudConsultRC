using Confluent.Kafka;

namespace CloudConsult.Common.Kafka
{
    public interface IKafkaProducer<TValue> where TValue : class
    {
        Task<DeliveryResult<Null, TValue>> ProduceAsync(string topic, TValue value, CancellationToken token);
    }
}
