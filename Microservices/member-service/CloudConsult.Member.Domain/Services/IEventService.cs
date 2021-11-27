using CloudConsult.Member.Domain.Entities;
using MongoDB.Bson;

namespace CloudConsult.Member.Domain.Services
{
    public interface IEventService
    {
        Task<IEnumerable<MemberProfile>> GetUnpublishedNewProfiles(CancellationToken cancellationToken = default);
        Task<IEnumerable<MemberProfile>> GetUnpublishedUpdatedProfiles(CancellationToken cancellationToken = default);
        Task SetProfileCreatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default);
        Task SetProfileUpdatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default);
    }
}