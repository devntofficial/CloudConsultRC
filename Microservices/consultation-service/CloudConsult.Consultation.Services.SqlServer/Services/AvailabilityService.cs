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
    public class AvailabilityService : IAvailabilityService
    {
        private readonly ConsultationDbContext _db;

        public AvailabilityService(ConsultationDbContext db)
        {
            _db = db;
        }

        public async Task AddDoctorAvailabilities(IEnumerable<DoctorAvailabilityEntity> availabilities,
            CancellationToken cancellationToken = default)
        {
            var listAvailabilities = availabilities.ToList();
            var doctorId = listAvailabilities.First().DoctorId;
            var distinctDates = listAvailabilities.Select(x => x.Date).Distinct().ToList();

            var existingTimeSlotsForDate = await _db.DoctorAvailabilities
                .Where(x => x.DoctorId == doctorId && distinctDates.Contains(x.Date))
                .ToListAsync(cancellationToken);

            //remove existing time slots in a day and then add timeslots
            //this way if time slots for a day exists and doctor updates them
            //then duplication wont happen, we will override previously saved time slots
            if (existingTimeSlotsForDate.Any())
            {
                _db.DoctorAvailabilities.RemoveRange(existingTimeSlotsForDate);
            }

            await _db.DoctorAvailabilities.AddRangeAsync(listAvailabilities, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<DoctorAvailabilityEntity>> GetDoctorAvailability(string doctorId,
            CancellationToken cancellationToken = default)
        {
            return await _db.DoctorAvailabilities
                .Where(x => x.DoctorId == doctorId)
                .ToListAsync(cancellationToken);
        }
    }
}