namespace CloudConsult.Consultation.Domain.Events
{
    public class ConsultationCancelled : ConsultationBaseEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
