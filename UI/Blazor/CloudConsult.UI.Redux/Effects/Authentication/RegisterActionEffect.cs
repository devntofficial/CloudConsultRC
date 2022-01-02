using Blazored.LocalStorage;
using Blazored.SessionStorage;
using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Interfaces.Identity;
using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace CloudConsult.UI.Redux.Effects.Authentication
{
    public class RegisterActionEffect : Effect<RegisterAction>
    {
        private readonly IIdentityService identityService;
        private readonly ILocalStorageService localStorage;
        private readonly ISessionStorageService sessionStorage;
        private readonly HttpClient client;
        private readonly AuthenticationStateProvider authStateProvider;

        public RegisterActionEffect(IIdentityService identityService, ILocalStorageService localStorage, ISessionStorageService sessionStorage,
            HttpClient client, AuthenticationStateProvider authStateProvider)
        {
            this.identityService = identityService;
            this.localStorage = localStorage;
            this.sessionStorage = sessionStorage;
            this.client = client;
            this.authStateProvider = authStateProvider;
        }

        public override async Task HandleAsync(RegisterAction action, IDispatcher dispatcher)
        {
            var response = await identityService.Register(action.Data);
            if (response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
            }

            if (response.IsSuccess == false)
            {
                dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
                return;
            }

            var token = response.Payload.AccessToken;
            await sessionStorage.SetItemAsync("AccessToken", token);
            await sessionStorage.SetItemAsync("IdentityId", response.Payload.Id);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ((AuthStateProvider)authStateProvider).NotifyUserLogin(token);
            dispatcher.Dispatch(new RegisterSuccessAction(response.Payload.Id, action.Data));
        }
    }
}
