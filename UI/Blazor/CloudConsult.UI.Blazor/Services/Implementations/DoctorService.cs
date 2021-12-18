using CloudConsult.UI.Blazor.Helpers;
using CloudConsult.UI.Blazor.Models.Doctor;
using CloudConsult.UI.Blazor.Services.Interfaces;
using MudBlazor;
using System.Text.Json;

namespace CloudConsult.UI.Blazor.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly HttpClient client;
        private readonly ISnackbar snackbar;
        private readonly JsonSerializerOptions options;

        public DoctorService(HttpClient client, ISnackbar snackbar)
        {
            this.client = client;
            this.snackbar = snackbar;
            this.options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<ProfileModel>> CreateProfile(ProfileModel profile, CancellationToken cancellationToken)
        {
            var response = await client.PostAsJsonAsync(GatewayRoutes.DoctorService.CreateProfile, profile);
            if (response is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse<ProfileModel>>(options);
            if (json is null)
            {
                snackbar.Add("Unable to parse response from server", Severity.Error);
                return new();
            }

            return json;
        }

        public async Task<ApiResponse<ProfileModel>> UpdateProfile(ProfileModel profile, CancellationToken cancellationToken)
        {
            var response = await client.PutAsJsonAsync(GatewayRoutes.DoctorService.UpdateProfile.Replace("{ProfileId}", profile.ProfileId), profile);
            if (response is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse<ProfileModel>>(options);
            if (json is null)
            {
                snackbar.Add("Unable to parse response from server", Severity.Error);
                return new();
            }

            return json;
        }

        public async Task<ApiResponse<ProfileModel>> GetProfileById(string profileId, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync(GatewayRoutes.DoctorService.GetProfileById.Replace("{ProfileId}", profileId));
            if (response is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse<ProfileModel>>(options);
            if (json is null)
            {
                snackbar.Add("Unable to parse response from server", Severity.Error);
                return new();
            }

            return json;
        }

        public async Task<ApiResponse<ProfileModel>> GetProfileByIdentityId(string identityId, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync(GatewayRoutes.DoctorService.GetProfileByIdentityId.Replace("{IdentityId}", identityId));
            if (response is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse<ProfileModel>>(options);
            if (json is null)
            {
                snackbar.Add("Unable to parse response from server", Severity.Error);
                return new();
            }

            return json;
        }
    }
}
