namespace CloudConsult.Consultation.Domain.Events;

public record ConsultationBooked
{
    public Guid Id { get; set; }
    public string DoctorProfileId { get; set; }
    public string DoctorName { get; set; }
    public string DoctorEmailId { get; set; }
    public string PatientProfileId { get; set; }
    public string PatientName { get; set; }
    public string PatientEmailId { get; set; }
    public string BookingDate { get; set; }
    public string BookingTimeSlot { get; set; }
    public string Description { get; set; }
}