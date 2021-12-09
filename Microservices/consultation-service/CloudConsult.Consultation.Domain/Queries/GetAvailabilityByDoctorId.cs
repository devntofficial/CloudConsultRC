using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Queries;

public class GetAvailabilityByDoctorId : IQuery<DoctorAvailabilityResponse>
{
    public string DoctorId { get; set; }
}