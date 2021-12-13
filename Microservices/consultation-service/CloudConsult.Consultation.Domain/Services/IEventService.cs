using CloudConsult.Consultation.Domain.Events;

namespace CloudConsult.Consultation.Domain.Services;

public interface IEventService
{
    Task<IEnumerable<ConsultationRequested>> GetPendingConsultationRequestedEvents(CancellationToken cancellationToken = default);
    Task<IEnumerable<ConsultationAccepted>> GetPendingConsultationAcceptedEvents(CancellationToken cancellationToken = default);
    Task<IEnumerable<ConsultationRejected>> GetPendingConsultationRejectedEvents(CancellationToken cancellationToken = default);
    Task<IEnumerable<ConsultationCancelled>> GetPendingConsultationCancelledEvents(CancellationToken cancellationToken = default);
    void SetEventPublished(string id);
    void SetReportUploadedEventConsumed(string id, string reportId);
}