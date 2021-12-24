using CloudConsult.UI.Blazor.Common;
using CloudConsult.UI.Blazor.Shared;
using CloudConsult.UI.Data.Authentication;
using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.States.Authentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CloudConsult.UI.Blazor.Pages.Authentication
{
    public class RegisterComponent : BaseComponent<RegisterState>
    {
        protected RegisterData data = new();
        protected bool AgreeToTerms = false;
        protected InputType PasswordInput = InputType.Password;
        protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private bool _passwordVisibility;

        [CascadingParameter]
        private Error Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Dispatcher.Dispatch(new GetRolesAction());
            Subscriber.SubscribeToAction<RegisterSuccessAction>(this, async action => await OnRegisterSuccess(action));
            await base.OnInitializedAsync();
        }

        private async Task OnRegisterSuccess(RegisterSuccessAction action)
        {
            switch(State.Value.Roles.FirstOrDefault(x => x.Id == data.RoleId).RoleName)
            {
                case "Administrator":
                    Navigation.NavigateTo($"/verify-otp/{action.IdentityId}");
                    return;
                case "Doctor":
                    await SessionStorage.SetItemAsync("FullName", action.Data.FullName);
                    await SessionStorage.SetItemAsync("EmailId", action.Data.EmailId);
                    Navigation.NavigateTo($"/doctor/{action.IdentityId}/profile/create");
                    return;
                case "Member":
                    await SessionStorage.SetItemAsync("FullName", action.Data.FullName);
                    await SessionStorage.SetItemAsync("EmailId", action.Data.EmailId);
                    Navigation.NavigateTo($"/member/{action.IdentityId}/profile/create");
                    return;
                default:
                    Notifier.Add("An unknown error occured");
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

        protected void RegisterButtonClick()
        {
            try
            {
                Dispatcher.Dispatch(new RegisterAction(data));
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
        }
    }
}