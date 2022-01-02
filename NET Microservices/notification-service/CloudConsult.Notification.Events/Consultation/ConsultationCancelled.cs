namespace CloudConsult.Notification.Events.Consultation
{
    public class ConsultationCancelled : ConsultationBaseEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
