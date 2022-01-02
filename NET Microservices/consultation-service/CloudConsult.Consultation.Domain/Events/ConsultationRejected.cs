namespace CloudConsult.Consultation.Domain.Events
{
    public class ConsultationRejected : ConsultationBaseEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
