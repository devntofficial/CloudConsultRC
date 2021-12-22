using Blazored.LocalStorage;
using Blazored.SessionStorage;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CloudConsult.UI.Blazor
{
    public class BaseComponent<T> : FluxorComponent
    {
        [Inject] protected IState<T> State { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        [Inject] protected IActionSubscriber Subscriber { get; set; }
        [Inject] protected ISnackbar Notifier { get; set; }
        [Inject] protected NavigationManager Navigation { get; set; }
        [Inject] protected ILocalStorageService LocalStorage { get; set; }
        [Inject] protected ISessionStorageService SessionStorage { get; set; }
        [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Subscriber.SubscribeToAction<GatewayErrorAction>(this, action => OnGatewayError(action));
            await base.OnInitializedAsync();
        }

        private void OnGatewayError(GatewayErrorAction action)
        {
            action.Errors.ForEach(message => Notifier.Add(message, Severity.Error));
        }

        protected override void Dispose(bool disposing)
        {
            Subscriber.UnsubscribeFromAllActions(this);
            base.Dispose(disposing);
        }
    }
}
