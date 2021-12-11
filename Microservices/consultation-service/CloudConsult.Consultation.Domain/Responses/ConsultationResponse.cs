namespace CloudConsult.Consultation.Domain.Responses;

public class ConsultationResponse
{
    public string DoctorProfileId { get; set; }
    public string DoctorName { get; set; }
    public string DoctorEmailId { get; set; }
    public string DoctorMobileNo { get; set; }
    public List<ConsultationData> Consultations { get; set; } = new();
}

public class ConsultationData
{
    public string Id { get; set; }
    public string MemberProfileId { get; set; }
    public string MemberName { get; set; }
    public string MemberEmailId { get; set; }
    public string MemberMobileNo { get; set; }
    public string BookingDate { get; set; }
    public string BookingTimeSlot { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
}
