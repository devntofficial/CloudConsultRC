using CloudConsult.UI.Models.Identity;
using CloudConsult.UI.Services.Interfaces;
using CloudConsult.UI.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CloudConsult.UI.Pages.Authentication
{
    public class RegisterComponent : ComponentBase
    {
        protected UserRegistrationModel registrationModel = new();
        protected List<UserRoleModel> rolesModel = new();
        protected bool AgreeToTerms;
        protected bool ShowLoadingSpinner;
        protected InputType PasswordInput = InputType.Password;
        protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private bool _passwordVisibility;

        [Inject]
        private IIdentityService IdentityService { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [CascadingParameter]
        private Error Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            rolesModel = await IdentityService.GetUserRoles();
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

        protected async Task RegisterButtonClick()
        {
            ShowLoadingSpinner = true;
            try
            {
                var isRegistered = await IdentityService.Register(registrationModel);
                if (isRegistered)
                {
                    Snackbar.Add("Your account was created successfully. Please login with your saved password.", Severity.Info);
                    Navigation.NavigateTo("/login");
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