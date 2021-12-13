using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Queries;

public class GetConsultationsByDoctorId : IQuery<ConsultationResponse>
{
    public string DoctorId { get; set; }
}