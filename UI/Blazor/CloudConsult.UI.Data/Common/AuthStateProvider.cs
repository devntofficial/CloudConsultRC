using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CloudConsult.UI.Data.Common
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient client;
        private readonly ILocalStorageService localStorage;
        private readonly ISessionStorageService sessionStorage;
        private readonly AuthenticationState anonymous;

        public AuthStateProvider(HttpClient client, ILocalStorageService localStorage, ISessionStorageService sessionStorage)
        {
            this.client = client;
            this.localStorage = localStorage;
            this.sessionStorage = sessionStorage;
            anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await localStorage.GetItemAsync<string>("AccessToken");

            if (string.IsNullOrWhiteSpace(token))
            {
                token = await sessionStorage.GetItemAsync<string>("AccessToken");
                if (string.IsNullOrWhiteSpace(token))
                    return anonymous;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return new AuthenticationState(
                   new ClaimsPrincipal(
                   new ClaimsIdentity(JwtParser.ParseClaimsFromToken(token), "jwtAuthType")));
        }

        public void NotifyUserLogin(string token)
        {
            var user = new ClaimsPrincipal(
                       new ClaimsIdentity(JwtParser.ParseClaimsFromToken(token), "jwtAuthType"));

            var authState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            localStorage.ClearAsync();
            sessionStorage.ClearAsync();
            var authState = Task.FromResult(anonymous);
            client.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
