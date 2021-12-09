using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CloudConsult.UI.Helpers
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;

        public AuthStateProvider(HttpClient client, ILocalStorageService localStorage)
        {
            _client = client;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorage.GetItemAsync<string>("AccessToken");

            if(string.IsNullOrWhiteSpace(token))
            {
                return _anonymous;
            }

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
