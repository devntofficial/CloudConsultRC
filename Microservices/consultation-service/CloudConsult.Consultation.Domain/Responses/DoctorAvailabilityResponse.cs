namespace CloudConsult.Consultation.Domain.Responses;

public record DoctorAvailabilityResponse
{
    public string DoctorId { get; set; }
    public Dictionary<string, List<string>> AvailabilityMap { get; set; }
}