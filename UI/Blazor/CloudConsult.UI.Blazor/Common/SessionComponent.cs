using Blazored.LocalStorage;
using Blazored.SessionStorage;
using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace CloudConsult.UI.Blazor.Common
{
    public class SessionComponent<T> : FluxorComponent
    {
        [Inject] protected IState<T> State { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        [Inject] protected IActionSubscriber Subscriber { get; set; }
        [Inject] protected ISnackbar Notifier { get; set; }
        [Inject] protected NavigationManager Navigation { get; set; }
        [Inject] protected ILocalStorageService LocalStorage { get; set; }
        [Inject] protected ISessionStorageService SessionStorage { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] protected IJSRuntime JavaScript { get; set; }
        protected string IdentityId { get; set; }
        protected string ProfileId { get; set; }
        protected string Role { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Subscriber.SubscribeToAction<LogoutAction>(this, action => OnLogout(action));
            Subscriber.SubscribeToAction<GatewayErrorAction>(this, action => OnGatewayError(action));

            ProfileId = await LocalStorage.GetItemAsync<string>("IdentityId");
            ProfileId = await SessionStorage.GetItemAsync<string>("ProfileId");
            Role = await SessionStorage.GetItemAsync<string>("Role");

            if (string.IsNullOrWhiteSpace(ProfileId) || string.IsNullOrWhiteSpace(Role))
            {
                //reset application
                Dispatcher.Dispatch(new LogoutAction());
            }
            await base.OnInitializedAsync();
        }

        private void OnGatewayError(GatewayErrorAction action)
        {
            action.Errors.ForEach(message => Notifier.Add(message, Severity.Error));
        }

        private void OnLogout(LogoutAction action)
        {
            ((AuthStateProvider)AuthStateProvider).NotifyUserLogout();
            Notifier.Add("Your session got expired. Please login again.", Severity.Error);
            Navigation.NavigateTo("/");
        }

        protected override void Dispose(bool disposing)
        {
            Subscriber.UnsubscribeFromAllActions(this);
            base.Dispose(disposing);
        }
    }
}
