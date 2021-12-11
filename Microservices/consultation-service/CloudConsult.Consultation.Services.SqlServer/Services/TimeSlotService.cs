using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;
using CloudConsult.Consultation.Domain.Services;
using CloudConsult.Consultation.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CloudConsult.Consultation.Services.SqlServer.Services
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly ConsultationDbContext db;

        public TimeSlotService(ConsultationDbContext db)
        {
            this.db = db;
        }

        public async Task AddDoctorAvailabilities(IEnumerable<DoctorTimeSlot> availabilities,
            CancellationToken cancellationToken = default)
        {
            var listAvailabilities = availabilities.ToList();
            var doctorId = listAvailabilities.First().DoctorProfileId;
            var distinctDates = listAvailabilities.Select(x => x.TimeSlotStart.Date).Distinct().ToList();

            var existingTimeSlotsForDate = await db.DoctorTimeSlots
                .Where(x => x.DoctorProfileId == doctorId && distinctDates.Contains(x.TimeSlotStart.Date))
                .ToListAsync(cancellationToken);

            //remove existing time slots in a day and then add timeslots
            //this way if time slots for a day exists and doctor updates them
            //then duplication wont happen, we will override previously saved time slots
            if (existingTimeSlotsForDate.Any())
            {
                db.DoctorTimeSlots.RemoveRange(existingTimeSlotsForDate);
            }

            await db.DoctorTimeSlots.AddRangeAsync(listAvailabilities, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<DoctorTimeSlot>> GetDoctorAvailability(string doctorId,
            CancellationToken cancellationToken = default)
        {
            return await db.DoctorTimeSlots
                .Where(x => x.DoctorProfileId == doctorId)
                .ToListAsync(cancellationToken);
        }
    }
}