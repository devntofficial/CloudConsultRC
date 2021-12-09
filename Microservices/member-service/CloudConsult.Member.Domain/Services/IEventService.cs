using CloudConsult.Member.Domain.Entities;
using MongoDB.Bson;

namespace CloudConsult.Member.Domain.Services
{
    public interface IEventService
    {
        Task<IEnumerable<MemberProfile>> GetUnpublishedNewProfiles(CancellationToken cancellationToken = default);
        Task<IEnumerable<MemberProfile>> GetUnpublishedUpdatedProfiles(CancellationToken cancellationToken = default);
        void SetProfileCreatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default);
        void SetProfileUpdatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default);
    }
}