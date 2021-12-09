namespace CloudConsult.Consultation.Domain.Responses;

public class GetConsultationByIdResponse
{
    public string Id { get; set; }
    public string DoctorId { get; set; }
    public string DoctorName { get; set; }
    public string PatientId { get; set; }
    public string PatientName { get; set; }
    public string BookingDate { get; set; }
    public string BookingTimeSlot { get; set; }
    public string Status { get; set; }
}