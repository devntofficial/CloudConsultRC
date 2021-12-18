using Confluent.Kafka;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudConsult.Common.Kafka
{
    public sealed class KafkaDeserializer<T> : IDeserializer<T>
    {
        JsonSerializerOptions options;

        public KafkaDeserializer()
        {
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                NumberHandling = JsonNumberHandling.Strict
            };
        }

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            if (typeof(T) == typeof(Null))
            {
                if (data.Length > 0)
                    throw new ArgumentException("The data is null");
                return default;
            }

            if (typeof(T) == typeof(Ignore))
                return default;

            return JsonSerializer.Deserialize<T>(data, options);
        }
    }
}