namespace CloudConsult.Consultation.Domain.Responses;

public class TimeSlotResponse
{
    public string DoctorId { get; set; }
    public Dictionary<string, List<string>> AvailabilityMap { get; set; }
}