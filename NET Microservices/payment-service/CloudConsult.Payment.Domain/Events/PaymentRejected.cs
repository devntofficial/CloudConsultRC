using CloudConsult.Payment.Domain.Responses;
using Kafka.Public;
using System.Text.Json;

namespace CloudConsult.Payment.Domain.Events
{
    public class PaymentRejected : PaymentResponse, IMemorySerializable
    {
        public void Serialize(MemoryStream toStream)
        {
            JsonSerializer.Serialize(toStream, this);
        }
    }
}
