using Microsoft.AspNetCore.Components;

namespace CloudConsult.UI.Blazor.Pages.Authentication
{
    public class ForgotPasswordComponent : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; }
        
        protected void GoBackToLogin()
        {
            Navigation.NavigateTo("/");
        }
    }
}