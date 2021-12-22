using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Data.Doctor;

namespace CloudConsult.UI.Interfaces.Doctor
{
    public interface IProfileService
    {
        Task<ApiResponse<ProfileResponseData>> CreateProfile(ProfileData profile, CancellationToken cancellationToken = default);
        Task<ApiResponse<ProfileResponseData>> UpdateProfile(string profileId, ProfileData profile, CancellationToken cancellationToken = default);
        Task<ApiResponse<ProfileResponseData>> GetProfileById(string profileId, CancellationToken cancellationToken = default);
        Task<ApiResponse<ProfileResponseData>> GetProfileByIdentityId(string identityId, CancellationToken cancellationToken = default);
    }
}
