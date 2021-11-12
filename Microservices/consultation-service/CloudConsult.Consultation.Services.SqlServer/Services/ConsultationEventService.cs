using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Services;
using CloudConsult.Consultation.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CloudConsult.Consultation.Services.SqlServer.Services
{
    public class ConsultationEventService : IConsultationEventService
    {
        private readonly ConsultationDbContext _db;

        public ConsultationEventService(ConsultationDbContext db)
        {
            _db = db;
        }
        
        public async Task<IEnumerable<ConsultationBookingEntity>> GetUnpublishedBookingEvents(CancellationToken cancellationToken = default)
        {
            return await _db.ConsultationBookings.Where(x => !x.IsBookingEventPublished).ToListAsync(cancellationToken);
        }

        public async Task UpdateBookingEventPublished(Guid id, CancellationToken cancellationToken = default)
        {
            var booking = await _db.ConsultationBookings
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync(cancellationToken);

            if (booking != null)
            {
                booking.IsBookingEventPublished = true;
                await _db.SaveChangesAsync(cancellationToken);
            }
        }
        
    }
}