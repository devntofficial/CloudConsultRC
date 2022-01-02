using AutoMapper;
using CloudConsult.Consultation.Domain.Commands;
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
        private readonly IMapper mapper;

        public TimeSlotService(ConsultationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task AddDoctorTimeSlots(IEnumerable<DoctorTimeSlot> availabilities,
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

        public async Task<(TimeSlot response, bool isSuccess, string message)> AddSingleTimeSlot(AddSingleTimeSlot command, CancellationToken cancellationToken = default)
        {
            var exists = await db.DoctorTimeSlots.AnyAsync(x => x.DoctorProfileId == command.ProfileId
            && x.TimeSlotStart == command.TimeSlotStart && x.TimeSlotEnd == command.TimeSlotEnd, cancellationToken);

            if(exists)
            {
                return (null, false, "Timeslot already exists");
            }

            var timeSlot = mapper.Map<DoctorTimeSlot>(command);
            await db.DoctorTimeSlots.AddAsync(timeSlot, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return (mapper.Map<TimeSlot>(timeSlot), true, "Saved successfully");
        }

        public async Task<IEnumerable<DoctorTimeSlot>> GetDoctorTimeSlots(string doctorId,
            CancellationToken cancellationToken = default)
        {
            return await db.DoctorTimeSlots
                .Where(x => x.DoctorProfileId == doctorId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<DoctorTimeSlot>> GetDoctorTimeSlotsRange(string doctorId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await db.DoctorTimeSlots
                .Where(x => x.DoctorProfileId == doctorId && x.TimeSlotStart >= startDate && x.TimeSlotEnd <= endDate)
                .ToListAsync(cancellationToken);
        }
    }
}