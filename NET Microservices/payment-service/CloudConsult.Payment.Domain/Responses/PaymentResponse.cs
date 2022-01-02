namespace CloudConsult.Payment.Domain.Responses
{
    public class PaymentResponse
    {
        public string PaymentId { get; set; } = Guid.NewGuid().ToString();
        public string ConsultationId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
