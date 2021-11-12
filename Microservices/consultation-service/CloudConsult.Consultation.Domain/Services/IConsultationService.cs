using System;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Services
{
    public interface IConsultationService
    {
        Task<String> BookConsultation(ConsultationBookingEntity booking, CancellationToken cancellationToken = default);
        Task<GetConsultationByIdResponse> GetById(string id, CancellationToken cancellationToken = default);
    }
}