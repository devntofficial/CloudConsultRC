using CloudConsult.UI.Blazor.Helpers;
using CloudConsult.UI.Blazor.Models.Identity;
using CloudConsult.UI.Blazor.Services.Interfaces;
using MudBlazor;
using System.Text.Json;

namespace CloudConsult.UI.Blazor.Services.Implementations
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient client;
        private readonly ISnackbar snackbar;
        private readonly JsonSerializerOptions options;

        public IdentityService(HttpClient client, ISnackbar snackbar)
        {
            this.client = client;
            this.snackbar = snackbar;
            this.options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<List<RoleModel>>> GetUserRoles()
        {
            var response = await client.GetAsync(GatewayRoutes.IdentityService.GetUserRoles);
            if (response is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse<List<RoleModel>>>(options);
            if (json is null)
            {
                snackbar.Add("Unable to parse response from server", Severity.Error);
                return new();
            }

            return json;
        }

        public async Task<ApiResponse<TokenModel>> GetToken(LoginModel userLoginData)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, GatewayRoutes.IdentityService.GetToken);
            request.Headers.Add("EmailId", userLoginData.EmailId);
            request.Headers.Add("Password", userLoginData.Password);

            var response = await client.SendAsync(request);
            if (response is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse<TokenModel>>(options);
            if (json is null)
            {
                snackbar.Add("Unable to parse response from server", Severity.Error);
                return new();
            }

            return json;
        }

        public async Task<ApiResponse> Register(RegistrationModel data)
        {
            var response = await client.PostAsJsonAsync(GatewayRoutes.IdentityService.CreateUser, data);
            if (response is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse>(options);
            if (json is null)
            {
                snackbar.Add("Unable to parse response from server", Severity.Error);
                return new();
            }

            return json;
        }

        public async Task<ApiResponse> ValidateOtp(string identityId, int otp)
        {
            var response = await client.PostAsJsonAsync(GatewayRoutes.IdentityService.ValidateOtp, new
            {
                IdentityId = identityId,
                Otp = otp
            });
            if (response is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse>(options);
            if (json is null)
            {
                snackbar.Add("Unable to parse response from server", Severity.Error);
                return new();
            }

            return json;
        }

        public async Task<ApiResponse> GenerateOtp(string identityId)
        {
            var response = await client.PostAsJsonAsync(GatewayRoutes.IdentityService.GenerateOtp, new
            {
                IdentityId = identityId
            });
            if (response is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse>(options);
            if (json is null)
            {
                snackbar.Add("Unable to parse response from server", Severity.Error);
                return new();
            }

            return json;
        }
    }
}
