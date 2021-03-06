using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Data.Doctor;
using CloudConsult.UI.Interfaces.Doctor;
using CloudConsult.UI.Services.Routes;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudConsult.UI.Services.Doctor
{
    public class ProfileService : IProfileService
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;

        public ProfileService(HttpClient client)
        {
            this.client = client;
            this.options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<ProfileResponseData>> CreateProfile(ProfileData profile, CancellationToken cancellationToken = default)
        {
            var response = await client.PostAsJsonAsync(GatewayRoutes.DoctorService.CreateProfile, profile, cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<ProfileResponseData>>(options, cancellationToken);
        }

        public async Task<ApiResponse<ProfileResponseData>> UpdateProfile(string profileId, ProfileData profile, CancellationToken cancellationToken = default)
        {
            var response = await client.PutAsJsonAsync(GatewayRoutes.DoctorService
                .UpdateProfile.Replace("{ProfileId}", profileId), profile, cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<ProfileResponseData>>(options, cancellationToken);
        }

        public async Task<ApiResponse<ProfileResponseData>> GetProfileById(string profileId, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync(GatewayRoutes.DoctorService.GetProfileById.Replace("{ProfileId}", profileId), cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<ProfileResponseData>>(options, cancellationToken);
        }

        public async Task<ApiResponse<ProfileResponseData>> GetProfileByIdentityId(string identityId, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync(GatewayRoutes.DoctorService.GetProfileByIdentityId.Replace("{IdentityId}", identityId), cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<ProfileResponseData>>(options, cancellationToken);
        }
    }
}
