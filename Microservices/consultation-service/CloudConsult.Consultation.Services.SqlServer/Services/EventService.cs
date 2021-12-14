using AutoMapper;
using CloudConsult.Common.Enums;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Events;
using CloudConsult.Consultation.Domain.Services;
using CloudConsult.Consultation.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CloudConsult.Consultation.Services.SqlServer.Services
{
    public class EventService : IEventService
    {
        private readonly ConsultationDbContext db;
        private readonly IMapper mapper;

        public EventService(ConsultationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ConsultationRequested>> GetPendingConsultationRequestedEvents(CancellationToken cancellationToken = default)
        {
            var events = await db.ConsultationEvents
                .Include(x => x.Consultation).Include(x => x.Consultation.TimeSlot)
                .Where(x => !x.IsEventPublished && x.EventName == ConsultationEvents.ConsultationRequested.ToString())
                .ToListAsync(cancellationToken);

            return mapper.Map<List<ConsultationRequested>>(events);
        }

        public async Task<IEnumerable<ConsultationAccepted>> GetPendingConsultationAcceptedEvents(CancellationToken cancellationToken = default)
        {
            return mapper.Map<List<ConsultationAccepted>>(await db.ConsultationEvents.Include(x => x.Consultation)
                .Where(x => !x.IsEventPublished && x.EventName == ConsultationEvents.ConsultationAccepted.ToString())
                .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<ConsultationRejected>> GetPendingConsultationRejectedEvents(CancellationToken cancellationToken = default)
        {
            return mapper.Map<List<ConsultationRejected>>(await db.ConsultationEvents.Include(x => x.Consultation)
                .Where(x => !x.IsEventPublished && x.EventName == ConsultationEvents.ConsultationRejected.ToString())
                .ToListAsync(cancellationToken));
        }

        public async Task<IEnumerable<ConsultationCancelled>> GetPendingConsultationCancelledEvents(CancellationToken cancellationToken = default)
        {
            return mapper.Map<List<ConsultationCancelled>>(await db.ConsultationEvents.Include(x => x.Consultation)
                .Where(x => !x.IsEventPublished && x.EventName == ConsultationEvents.ConsultationCancelled.ToString())
                .ToListAsync(cancellationToken));
        }

        public void SetEventPublished(string id)
        {
            var booking = db.ConsultationEvents.FirstOrDefault(x => x.Id == id);

            if (booking is not null)
            {
                booking.IsEventPublished = true;
                db.SaveChanges();
            }
        }

        public void SetReportUploadedEventConsumed(string id, string reportId)
        {
            var consultation = db.ConsultationRequests.FirstOrDefault(x => x.Id == id);

            if (consultation is not null)
            {
                db.ConsultationEvents.Add(new ConsultationEvent
                {
                    ConsultationId = consultation.Id,
                    EventName = ConsultationEvents.DiagnosisReportPublished.ToString(),
                    IsEventPublished = true,
                    Timestamp = DateTime.Now
                });

                consultation.DiagnosisReportId = reportId;
                consultation.IsComplete = true;
                consultation.Status = ConsultationEvents.ProcessComplete.ToString();

                db.SaveChanges();
            }
        }

        public void SetPaymentAcceptedEventConsumed(string id, string paymentId)
        {
            var consultation = db.ConsultationRequests.FirstOrDefault(x => x.Id == id);

            if (consultation is not null)
            {
                db.ConsultationEvents.Add(new ConsultationEvent
                {
                    ConsultationId = consultation.Id,
                    EventName = ConsultationEvents.PaymentAccepted.ToString(),
                    IsEventPublished = true,
                    Timestamp = DateTime.Now
                });

                consultation.PaymentId = paymentId;
                consultation.Status = ConsultationEvents.PaymentAccepted.ToString();

                db.SaveChanges();
            }
        }

        public void SetPaymentRejectedEventConsumed(string id)
        {
            var consultation = db.ConsultationRequests.FirstOrDefault(x => x.Id == id);

            if (consultation is not null)
            {
                db.ConsultationEvents.Add(new ConsultationEvent
                {
                    ConsultationId = consultation.Id,
                    EventName = ConsultationEvents.PaymentRejected.ToString(),
                    IsEventPublished = true,
                    Timestamp = DateTime.Now
                });

                consultation.Status = ConsultationEvents.PaymentRejected.ToString();

                db.SaveChanges();
            }
        }
    }
}