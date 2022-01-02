using CloudConsult.Doctor.Domain.Entities;

namespace CloudConsult.Doctor.Domain.Services
{
    public interface IProfileService
    {
        Task<DoctorProfile> Create(DoctorProfile profile, CancellationToken cancellationToken = default);
        Task<DoctorProfile> Update(DoctorProfile profile, CancellationToken cancellationToken = default);
        Task<List<DoctorProfile>> GetAllPaginated(int pageNo, int pageSize, CancellationToken cancellationToken = default);
        Task<DoctorProfile> GetById(string profileId, CancellationToken cancellationToken = default);
        Task<DoctorProfile> GetByIdentityId(string identityId, CancellationToken cancellationToken = default);
    }
}