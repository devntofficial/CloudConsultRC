using CloudConsult.UI.Blazor.Models.Identity;
using CloudConsult.UI.Blazor.Services.Interfaces;
using CloudConsult.UI.Blazor.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CloudConsult.UI.Blazor.Pages.Authentication
{
    public class RegisterComponent : ComponentBase
    {
        protected RegistrationModel RegistrationModel = new();
        protected List<RoleModel> RolesModel = new();
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
            var output = await IdentityService.GetUserRoles();
            if(output.IsSuccess)
            {
                RolesModel = output.Payload;
            }
            else
            {
                for (int i = 0; i < output.Errors.Count(); i++)
                {
                    Snackbar.Add(output.Errors.ElementAt(i), Severity.Error);
                }
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

        protected async Task RegisterButtonClick()
        {
            ShowLoadingSpinner = true;
            try
            {
                var output = await IdentityService.Register(RegistrationModel);
                if (output.IsSuccess)
                {
                    Snackbar.Add("Your account was created successfully. Please login with your saved password.", Severity.Info);
                    Navigation.NavigateTo("/verify-otp");
                }
                else
                {
                    for (int i = 0; i < output.Errors.Count(); i++)
                    {
                        Snackbar.Add(output.Errors.ElementAt(i), Severity.Error);
                    }
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