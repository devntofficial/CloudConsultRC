using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Consultation.Domain.Entities;

namespace CloudConsult.Consultation.Domain.Services
{
    public interface IAvailabilityService
    {
        Task AddDoctorAvailabilities(IEnumerable<DoctorAvailabilityEntity> availabilities,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<DoctorAvailabilityEntity>> GetDoctorAvailability(string doctorId,
            CancellationToken cancellationToken = default);
    }
}