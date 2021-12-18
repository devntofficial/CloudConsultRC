using Confluent.Kafka;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudConsult.Common.Kafka
{
    public sealed class KafkaSerializer<T> : ISerializer<T>
    {
        JsonSerializerOptions options;

        public KafkaSerializer()
        {
            options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                NumberHandling = JsonNumberHandling.Strict
            };
        }

        public byte[] Serialize(T data, SerializationContext context)
        {
            if (typeof(T) == typeof(Null))
                return null;

            if (typeof(T) == typeof(Ignore))
                throw new NotSupportedException("Not Supported.");

            return JsonSerializer.SerializeToUtf8Bytes(data, options);
        }
    }
}
