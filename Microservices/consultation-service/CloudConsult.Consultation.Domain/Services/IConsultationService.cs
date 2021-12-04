using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Services
{
    public interface IConsultationService
    {
        Task<String> BookConsultation(ConsultationBooking booking, CancellationToken cancellationToken = default);
        Task<GetConsultationByIdResponse> GetById(string id, CancellationToken cancellationToken = default);
        Task<List<ConsultationBooking>> GetByDoctorId(string doctorId, CancellationToken cancellationToken = default);
    }
}