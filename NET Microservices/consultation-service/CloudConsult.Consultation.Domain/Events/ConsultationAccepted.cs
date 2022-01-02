namespace CloudConsult.Consultation.Domain.Events
{
    public class ConsultationAccepted : ConsultationBaseEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
