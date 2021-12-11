namespace CloudConsult.Notification.Events.Consultation;

public class ConsultationRequested : ConsultationBaseEvent
{
    public string TimeSlotId { get; set; } = string.Empty;
    public string BookingDate { get; set; } = string.Empty;
    public string BookingTimeSlot { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
}