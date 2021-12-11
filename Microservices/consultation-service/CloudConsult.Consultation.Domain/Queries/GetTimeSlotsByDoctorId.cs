using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Queries;

public class GetTimeSlotsByDoctorId : IQuery<TimeSlotResponse>
{
    public string DoctorId { get; set; }
}