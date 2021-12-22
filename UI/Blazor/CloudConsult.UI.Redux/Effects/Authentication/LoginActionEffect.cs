using Blazored.LocalStorage;
using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Interfaces.Identity;
using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CloudConsult.UI.Redux.Effects.Authentication
{
    public class LoginActionEffect : Effect<LoginAction>
    {
        private readonly IIdentityService identityService;
        private readonly ILocalStorageService localStorage;
        private readonly HttpClient client;
        private readonly AuthenticationStateProvider authStateProvider;

        public LoginActionEffect(IIdentityService identityService, ILocalStorageService localStorage,
            HttpClient client, AuthenticationStateProvider authStateProvider)
        {
            this.identityService = identityService;
            this.localStorage = localStorage;
            this.client = client;
            this.authStateProvider = authStateProvider;
        }

        public override async Task HandleAsync(LoginAction action, IDispatcher dispatcher)
        {
            var response = await identityService.GetToken(action.Data);
            if (response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
                return;
            }

            if (response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                dispatcher.Dispatch(new LoginUnverifiedAction(response.Payload.IdentityId));
                return;
            }

            if (response.IsSuccess == false)
            {
                dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
                return;
            }

            var token = response.Payload.AccessToken;
            await localStorage.SetItemAsync("AccessToken", token);
            await localStorage.SetItemAsync("IdentityId", response.Payload.IdentityId);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var provider = (AuthStateProvider)authStateProvider;
            provider.NotifyUserLogin(token);

            var state = await provider.GetAuthenticationStateAsync();
            var role = state.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

            dispatcher.Dispatch(new LoginSuccessAction(response.Payload.IdentityId, role));
        }
    }
}
