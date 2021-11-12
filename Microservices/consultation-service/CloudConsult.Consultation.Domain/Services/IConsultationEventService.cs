using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Consultation.Domain.Entities;

namespace CloudConsult.Consultation.Domain.Services
{
    public interface IConsultationEventService
    {
        Task<IEnumerable<ConsultationBookingEntity>> GetUnpublishedBookingEvents(CancellationToken cancellationToken = default);
        Task UpdateBookingEventPublished(Guid id, CancellationToken cancellationToken = default);
    }
}