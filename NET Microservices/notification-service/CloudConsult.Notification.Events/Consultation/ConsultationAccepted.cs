namespace CloudConsult.Notification.Events.Consultation
{
    public class ConsultationAccepted : ConsultationBaseEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
