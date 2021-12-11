using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Services;

public interface IConsultationService
{
    Task<ConsultationStatusResponse> Request(ConsultationRequest consultation, CancellationToken cancellationToken = default);
    Task<ConsultationStatusResponse> Accept(string consultationId, CancellationToken cancellationToken = default);
    Task<ConsultationStatusResponse> Reject(string consultationId, CancellationToken cancellationToken = default);
    Task<ConsultationStatusResponse> Cancel(string consultationId, CancellationToken cancellationToken = default);
    Task<ConsultationRequest> GetById(string consultationId, CancellationToken cancellationToken = default);
    Task<List<ConsultationRequest>> GetByDoctorProfileId(string doctorProfileId, CancellationToken cancellationToken = default);
}