using CloudConsult.UI.Blazor.Helpers;
using CloudConsult.UI.Blazor.Models.Doctor;

namespace CloudConsult.UI.Blazor.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<ApiResponse<ProfileModel>> CreateProfile(ProfileModel profile, CancellationToken cancellationToken);
        Task<ApiResponse<ProfileModel>> UpdateProfile(ProfileModel profile, CancellationToken cancellationToken);
        Task<ApiResponse<ProfileModel>> GetProfileById(string profileId, CancellationToken cancellationToken);
        Task<ApiResponse<ProfileModel>> GetProfileByIdentityId(string identityId, CancellationToken cancellationToken);
    }
}
