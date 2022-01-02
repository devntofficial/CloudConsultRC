using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Data.Member;

namespace CloudConsult.UI.Interfaces.Member
{
    public interface IProfileService
    {
        Task<ApiResponse<ProfileResponseData>> CreateProfile(ProfileData profile, CancellationToken cancellationToken = default);
        Task<ApiResponse<ProfileResponseData>> UpdateProfile(string profileId, ProfileData profile, CancellationToken cancellationToken = default);
        Task<ApiResponse<ProfileResponseData>> GetProfileById(string profileId, CancellationToken cancellationToken = default);
        Task<ApiResponse<ProfileResponseData>> GetProfileByIdentityId(string identityId, CancellationToken cancellationToken = default);
    }
}
