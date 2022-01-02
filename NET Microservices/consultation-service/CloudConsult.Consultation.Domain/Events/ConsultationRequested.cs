namespace CloudConsult.Consultation.Domain.Events;

public class ConsultationRequested : ConsultationBaseEvent
{
    public string TimeSlotId { get; set; }
    public string BookingDate { get; set; }
    public string BookingTimeSlot { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}