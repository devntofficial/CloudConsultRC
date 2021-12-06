using CloudConsult.Consultation.Domain.Entities;

namespace CloudConsult.Consultation.Domain.Services;

public interface IConsultationEventService
{
    Task<IEnumerable<ConsultationBooking>> GetUnpublishedBookingEvents(CancellationToken cancellationToken = default);
    Task UpdateBookingEventPublished(Guid id, CancellationToken cancellationToken = default);
}