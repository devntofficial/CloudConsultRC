using CloudConsult.Consultation.Domain.Entities;

namespace CloudConsult.Consultation.Domain.Services;

public interface IAvailabilityService
{
    Task AddDoctorAvailabilities(IEnumerable<DoctorAvailability> availabilities, CancellationToken cancellationToken = default);

    Task<IEnumerable<DoctorAvailability>> GetDoctorAvailability(string doctorId, CancellationToken cancellationToken = default);
}