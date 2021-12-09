using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Queries;

public record GetConsultationsByDoctorId : IQuery<ConsultationResponse>
{
    public string DoctorId { get; set; }
}