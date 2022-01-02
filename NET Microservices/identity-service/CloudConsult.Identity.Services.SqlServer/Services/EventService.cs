using AutoMapper;
using CloudConsult.Identity.Domain.Events;
using CloudConsult.Identity.Domain.Services;
using CloudConsult.Identity.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CloudConsult.Identity.Services.SqlServer.Services
{
    public class EventService : IEventService
    {
        private readonly IdentityDbContext db;
        private readonly IMapper mapper;

        public EventService(IdentityDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<OtpGenerated>> GetPendingOtpGeneratedEvents(CancellationToken cancellationToken = default)
        {
            var pendingEvents = await db.UserOtps
                .Include(x => x.User)
                .Where(x => !x.IsEventPublished)
                .ToListAsync(cancellationToken);
            
            return pendingEvents.Count == 0 ? null : mapper.Map<List<OtpGenerated>>(pendingEvents);
        }

        public void SetOtpGeneratedEventPublished(string eventId, bool value)
        {
            var otpEvent = db.UserOtps.FirstOrDefault(x => x.Id == eventId);
            if(otpEvent is not null)
            {
                otpEvent.IsEventPublished = value;
                db.SaveChanges();
            }
        }
    }
}
