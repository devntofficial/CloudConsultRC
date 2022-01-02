using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Queries;

public class GetConsultationById : IQuery<GetConsultationByIdResponse>
{
    public string ConsultationId { get; set; }
}