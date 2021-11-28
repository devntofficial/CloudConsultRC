using Blazored.LocalStorage;
using CloudConsult.UI.Helpers;
using CloudConsult.UI.Models.Identity;
using CloudConsult.UI.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudConsult.UI.Services.Implementations
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient client;
        private readonly AuthenticationStateProvider authStateProvider;
        private readonly ILocalStorageService localStorage;
        private readonly ISnackbar snackbar;

        public IdentityService(HttpClient client, AuthenticationStateProvider authStateProvider,
            ILocalStorageService localStorage, ISnackbar snackbar)
        {
            this.client = client;
            this.authStateProvider = authStateProvider;
            this.localStorage = localStorage;
            this.snackbar = snackbar;
        }

        public async Task<List<UserRoleModel>> GetUserRoles()
        {
            var response = await client.GetAsync(GatewayRoutes.IdentityService.GetUserRoles);
            var json = await response.Content
                .ReadFromJsonAsync<ApiResponseBuilder<List<UserRoleModel>>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (json is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            if (!json.IsSuccess)
            {
                for(int i = 0; i < json.Errors.Count(); i++)
                {
                    snackbar.Add(json.Errors.ElementAt(i), Severity.Error);
                }
                return new();
            }

            return json.Payload;
        }

        public async Task<bool> Login(LoginModel userLoginData)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, GatewayRoutes.IdentityService.GetToken);
            request.Headers.Add("EmailId", userLoginData.EmailId);
            request.Headers.Add("Password", userLoginData.Password);

            var response = await client.SendAsync(request);
            var json = await response.Content
                .ReadFromJsonAsync<ApiResponseBuilder<AuthenticatedUserModel>>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if(json is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return false;
            }

            await localStorage.SetItemAsync("AccessToken", json.Payload.AccessToken);
            ((AuthStateProvider)authStateProvider).NotifyUserLogin(json.Payload.AccessToken);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", json.Payload.AccessToken);

            return true;
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("AccessToken");
            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
            client.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> Register(UserRegistrationModel data)
        {
            var response = await client.PostAsJsonAsync(GatewayRoutes.IdentityService.CreateUser, data);
            var json = await response.Content
                .ReadFromJsonAsync<ApiResponseBuilder>(new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (json is null)
            {
                snackbar.Add("Unable to connect to server", Severity.Error);
                return new();
            }

            if (!json.IsSuccess)
            {
                for (int i = 0; i < json.Errors.Count(); i++)
                {
                    snackbar.Add(json.Errors.ElementAt(i), Severity.Error);
                }
                return new();
            }

            return true;
        }
    }
}
