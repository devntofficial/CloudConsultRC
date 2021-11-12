using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CloudConsult.UI.Pages.Authentication
{
    public class RegisterComponent : ComponentBase
    {
        protected string Password;
        protected bool AgreeToTerms;
        protected InputType PasswordInput = InputType.Password;
        protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        private bool _passwordVisibility;

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
    }
}