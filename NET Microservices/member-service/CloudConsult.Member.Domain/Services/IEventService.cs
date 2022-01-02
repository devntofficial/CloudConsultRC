using CloudConsult.Member.Domain.Events;

namespace CloudConsult.Member.Domain.Services
{
    public interface IEventService
    {
        Task<IEnumerable<ProfileCreated>> GetPendingProfileCreatedEvents(CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfileUpdated>> GetPendingProfileUpdatedEvents(CancellationToken cancellationToken = default);
        void SetProfileCreatedEventPublished(string profileId);
        void SetProfileUpdatedEventPublished(string profileId);
    }
}