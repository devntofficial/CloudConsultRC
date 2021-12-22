using CloudConsult.UI.Data.Authentication;
using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Interfaces.Identity;
using CloudConsult.UI.Services.Routes;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudConsult.UI.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;

        public IdentityService(HttpClient client)
        {
            this.client = client;
            this.options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<List<RoleData>>> GetUserRoles()
        {
            var response = await client.GetAsync(GatewayRoutes.IdentityService.GetUserRoles);
            if (response is null)
            {
                return null;
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse<List<RoleData>>>(options);
            if (json is null)
            {
                return null;
            }

            return json;
        }

        public async Task<ApiResponse<LoginResponseData>> GetToken(LoginData loginData)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, GatewayRoutes.IdentityService.GetToken);
            request.Headers.Add("EmailId", loginData.EmailId);
            request.Headers.Add("Password", loginData.Password);

            var response = await client.SendAsync(request);
            if (response is null)
            {
                return null;
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse<LoginResponseData>>(options);
            if (json is null)
            {
                return null;
            }

            return json;
        }

        public async Task<ApiResponse<RegisterResponseData>> Register(RegisterData data)
        {
            var response = await client.PostAsJsonAsync(GatewayRoutes.IdentityService.CreateUser, data);
            if (response is null)
            {
                return null;
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse<RegisterResponseData>>(options);
            if (json is null)
            {
                return null;
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
                return null;
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse>(options);
            if (json is null)
            {
                return null;
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
                return null;
            }

            var json = await response.Content.ReadFromJsonAsync<ApiResponse>(options);
            if (json is null)
            {
                return null;
            }

            return json;
        }
    }
}
