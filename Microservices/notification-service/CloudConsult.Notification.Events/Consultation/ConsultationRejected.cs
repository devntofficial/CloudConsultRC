namespace CloudConsult.Notification.Events.Consultation
{
    public class ConsultationRejected : ConsultationBaseEvent
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
