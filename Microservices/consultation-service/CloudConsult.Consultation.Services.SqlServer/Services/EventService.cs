using AutoMapper;
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

        public async Task<IEnumerable<ConsultationBooked>> GetPendingConsultationBookedEvents(CancellationToken cancellationToken = default)
        {
            return mapper.Map<List<ConsultationBooked>>(await db.ConsultationBookings
                .Where(x => !x.IsBookingEventPublished)
                .ToListAsync(cancellationToken));
        }

        public void SetConsultationBookedEventPublished(Guid id)
        {
            var booking = db.ConsultationBookings.Where(x => x.Id.Equals(id)).FirstOrDefault();

            if (booking != null)
            {
                booking.IsBookingEventPublished = true;
                db.SaveChanges();
            }
        }

    }
}