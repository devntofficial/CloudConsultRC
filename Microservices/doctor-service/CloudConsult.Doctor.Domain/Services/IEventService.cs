using CloudConsult.Doctor.Domain.Events;

namespace CloudConsult.Doctor.Domain.Services
{
    public interface IEventService
    {
        Task<IEnumerable<ProfileCreated>> GetPendingProfileCreatedEvents(CancellationToken cancellationToken = default);
        Task<IEnumerable<ProfileUpdated>> GetPendingProfileUpdatedEvents(CancellationToken cancellationToken = default);
        Task<IEnumerable<KycApproved>> GetPendingKycApprovedEvents(CancellationToken cancellationToken = default);
        Task<IEnumerable<KycRejected>> GetPendingKycRejectedEvents(CancellationToken cancellationToken = default);
        void SetProfileCreatedEventPublished(string profileId, CancellationToken cancellationToken = default);
        void SetProfileUpdatedEventPublished(string profileId, CancellationToken cancellationToken = default);
        void SetKycEventPublished(string eventId, CancellationToken cancellationToken = default);
    }
}