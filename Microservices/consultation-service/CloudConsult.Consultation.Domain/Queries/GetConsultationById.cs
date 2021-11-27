using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Queries
{
    public record GetConsultationById : IQuery<GetConsultationByIdResponse>
    {
        public string ConsultationId { get; set; }
    }
}