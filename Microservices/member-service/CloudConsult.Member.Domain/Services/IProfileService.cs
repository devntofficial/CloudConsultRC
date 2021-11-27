using CloudConsult.Member.Domain.Entities;

namespace CloudConsult.Member.Domain.Services
{
    public interface IProfileService
    {
        Task<MemberProfile> Create(MemberProfile profile, CancellationToken cancellationToken = default);
        Task<MemberProfile> Update(MemberProfile profile, CancellationToken cancellationToken = default);
        Task<MemberProfile> GetById(string profileId, CancellationToken cancellationToken = default);
        Task<MemberProfile> GetByIdentityId(string identityId, CancellationToken cancellationToken = default);
    }
}