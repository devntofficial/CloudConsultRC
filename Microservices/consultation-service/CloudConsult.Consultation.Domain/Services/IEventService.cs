using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Events;

namespace CloudConsult.Consultation.Domain.Services;

public interface IEventService
{
    Task<IEnumerable<ConsultationBooked>> GetPendingConsultationBookedEvents(CancellationToken cancellationToken = default);
    void SetConsultationBookedEventPublished(Guid id);
}