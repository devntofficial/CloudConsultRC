using CloudConsult.Consultation.Domain.Entities;

namespace CloudConsult.Consultation.Domain.Services;

public interface ITimeSlotService
{
    Task AddDoctorAvailabilities(IEnumerable<DoctorTimeSlot> availabilities, CancellationToken cancellationToken = default);

    Task<IEnumerable<DoctorTimeSlot>> GetDoctorAvailability(string doctorId, CancellationToken cancellationToken = default);
}