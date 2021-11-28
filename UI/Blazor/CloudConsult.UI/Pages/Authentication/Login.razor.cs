using CloudConsult.UI.Models.Identity;
using CloudConsult.UI.Services.Interfaces;
using CloudConsult.UI.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CloudConsult.UI.Pages.Authentication
{
    public class LoginComponent : ComponentBase
    {
        protected readonly LoginModel LoginModel = new();
        protected InputType PasswordInput = InputType.Password;
        protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        protected bool ShowLoadingSpinner;
        private bool _passwordVisibility;

        [Inject]
        private IIdentityService AuthService { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [CascadingParameter]
        private Error Error { get; set; }

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

        protected async Task LoginButtonClick()
        {
            ShowLoadingSpinner = true;
            try
            {
                var result = await AuthService.Login(LoginModel);
                if (result is true)
                {
                    Navigation.NavigateTo("/dashboard");
                }
                else
                {
                    Snackbar.Add("Invalid email/password combination", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
            ShowLoadingSpinner = false;
        }
    }
}
