using CloudConsult.UI.Blazor.Shared;
using CloudConsult.UI.Data.Authentication;
using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.States.Authentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Security.Claims;

namespace CloudConsult.UI.Blazor.Pages.Authentication
{
    public class LoginComponent : BaseComponent<LoginState>
    {
        protected LoginData data = new();
        protected InputType PasswordInput = InputType.Password;
        protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private bool _passwordVisibility;

        [CascadingParameter] private Error error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Subscriber.SubscribeToAction<LoginSuccessAction>(this, action => OnLoginSuccess(action));
            Subscriber.SubscribeToAction<LoginUnverifiedAction>(this, action => OnLoginUnverified(action));

            var state = await ((AuthStateProvider)AuthStateProvider).GetAuthenticationStateAsync();
            if(state.User.HasClaim(x => x.Type == ClaimTypes.Role))
            {
                var id = state.User.Claims.FirstOrDefault(x => x.Type == "Id").Value;
                var role = state.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
                Dispatcher.Dispatch(new LoginSuccessAction(id, role));
            }

            await base.OnInitializedAsync();
        }

        private void OnLoginUnverified(LoginUnverifiedAction action)
        {
            Navigation.NavigateTo($"/verify-otp/{action.IdentityId}");
        }

        private void OnLoginSuccess(LoginSuccessAction action)
        {
            SessionStorage.SetItemAsync("Role", action.Role);
            switch (action.Role)
            {
                default: Notifier.Add("Not Allowed", Severity.Error);
                    return;
                case "Administrator":
                case "Doctor":
                case "Member":
                    Navigation.NavigateTo($"/{action.Role.ToLower()}/{action.IdentityId}/dashboard");
                    return;
            }
            
        }

        protected void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        protected void LoginButtonClick()
        {
            try
            {
                Dispatcher.Dispatch(new LoginAction(data));
            }
            catch (Exception ex)
            {
                error.ProcessError(ex);
            }
        }
    }
}
