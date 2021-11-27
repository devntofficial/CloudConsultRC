using CloudConsult.Doctor.Domain.Entities;
using MongoDB.Bson;

namespace CloudConsult.Doctor.Domain.Services
{
    public interface IEventService
    {
        Task<IEnumerable<DoctorProfile>> GetUnpublishedNewProfiles(CancellationToken cancellationToken = default);
        Task<IEnumerable<DoctorProfile>> GetUnpublishedUpdatedProfiles(CancellationToken cancellationToken = default);
        Task SetProfileCreatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default);
        Task SetProfileUpdatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default);
    }
}