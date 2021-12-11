namespace CloudConsult.Consultation.Domain.Responses;

public record TimeSlotResponse
{
    public string DoctorId { get; set; }
    public Dictionary<string, List<string>> AvailabilityMap { get; set; }
}