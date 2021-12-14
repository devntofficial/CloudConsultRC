namespace CloudConsult.Consultation.Domain.Events
{
    public class PaymentRejected
    {
        public string PaymentId { get; set; } = string.Empty;
        public string ConsultationId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
