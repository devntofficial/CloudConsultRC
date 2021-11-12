using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CloudConsult.UI.Helpers;
using CloudConsult.UI.Models;
using CloudConsult.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace CloudConsult.UI.Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(HttpClient client,
            AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<AuthenticatedUserModel> Login(LoginModel userLoginData)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, GatewayRoutes.IdentityService.GetToken);
            request.Headers.Add("EmailId", userLoginData.EmailId);
            request.Headers.Add("Password", userLoginData.Password);

            var authResult = await _client.SendAsync(request);
            var authContent = await authResult.Content.ReadAsStreamAsync();

            if (authResult.IsSuccessStatusCode == false)
            {
                return null;
            }

            var result = await JsonSerializer.DeserializeAsync<ApiResponseBuilder<AuthenticatedUserModel>>(authContent,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result == null)
            {
                return null;
            }
                
            await _localStorage.SetItemAsync("AccessToken", result.Payload.AccessToken);
            ((AuthStateProvider) _authStateProvider).NotifyUserLogin(result.Payload.AccessToken);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.Payload.AccessToken);
            
            return result.Payload;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("AccessToken");
            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();
            _client.DefaultRequestHeaders.Authorization = null;
        }
    }
}
