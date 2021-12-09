using CloudConsult.Common.Enums;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;
using CloudConsult.Consultation.Domain.Services;
using CloudConsult.Consultation.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CloudConsult.Consultation.Services.SqlServer.Services
{
    public class ConsultationService : IConsultationService
    {
        private readonly ConsultationDbContext _db;

        public ConsultationService(ConsultationDbContext db)
        {
            this._db = db;
        }

        public async Task<string> BookConsultation(ConsultationBooking booking, CancellationToken cancellationToken = default)
        {
            var timeSlot = await _db.DoctorAvailabilities
                .Where(x => x.DoctorId == booking.DoctorProfileId &&
                            x.TimeSlotStart == booking.BookingStartDateTime &&
                            x.TimeSlotEnd == booking.BookingEndDateTime)
                .SingleOrDefaultAsync(cancellationToken);

            if (timeSlot is null)
            {
                return "Invalid time slot provided";
            }

            if (timeSlot.IsBooked)
            {
                return "This time slot is already booked for the given doctor id";
            }

            booking.TimeSlotId = timeSlot.Id;
            booking.Status = ConsultationStatus.DoctorApprovalPending.ToString();

            await _db.ConsultationBookings.AddAsync(booking, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return booking.Id.ToString();
        }

        public async Task<List<ConsultationBooking>> GetByDoctorId(string doctorId, CancellationToken cancellationToken = default)
        {
            return await _db.ConsultationBookings.Where(x => x.DoctorProfileId.Equals(doctorId)).ToListAsync(cancellationToken);
        }

        public async Task<GetConsultationByIdResponse> GetById(string id, CancellationToken cancellationToken = default)
        {
            var consultation = await _db.ConsultationBookings
                .Where(x => Guid.Parse(id).Equals(x.Id))
                .Select(x => new GetConsultationByIdResponse
                {
                    Id = x.Id.ToString(),
                    DoctorId = x.DoctorProfileId,
                    DoctorName = x.DoctorName,
                    PatientId = x.PatientProfileId,
                    PatientName = x.PatientName,
                    BookingDate = x.BookingStartDateTime.ToString("dd-MM-yyyy"),
                    BookingTimeSlot = $"{x.BookingStartDateTime:HH:mm}-{x.BookingEndDateTime:HH:mm}",
                    Status = x.Status
                })
                .SingleOrDefaultAsync(cancellationToken);
            return consultation;
        }
    }
}