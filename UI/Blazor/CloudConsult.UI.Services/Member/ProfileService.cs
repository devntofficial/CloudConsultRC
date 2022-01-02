using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Data.Member;
using CloudConsult.UI.Interfaces.Member;
using CloudConsult.UI.Services.Routes;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudConsult.UI.Services.Member
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
            var response = await client.PostAsJsonAsync(GatewayRoutes.MemberService.CreateProfile, profile, cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<ProfileResponseData>>(options, cancellationToken);
        }

        public async Task<ApiResponse<ProfileResponseData>> GetProfileById(string profileId, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync(GatewayRoutes.MemberService.GetProfileById.Replace("{ProfileId}", profileId), cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<ProfileResponseData>>(options, cancellationToken);
        }

        public async Task<ApiResponse<ProfileResponseData>> GetProfileByIdentityId(string identityId, CancellationToken cancellationToken = default)
        {
            var response = await client.GetAsync(GatewayRoutes.MemberService.GetProfileByIdentityId.Replace("{IdentityId}", identityId), cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<ProfileResponseData>>(options, cancellationToken);
        }

        public async Task<ApiResponse<ProfileResponseData>> UpdateProfile(string profileId, ProfileData profile, CancellationToken cancellationToken = default)
        {
            var response = await client.PutAsJsonAsync(GatewayRoutes.MemberService.UpdateProfile.Replace("{ProfileId}", profileId), profile, cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<ProfileResponseData>>(options, cancellationToken);
        }
    }
}
