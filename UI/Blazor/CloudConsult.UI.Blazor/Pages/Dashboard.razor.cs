using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CloudConsult.UI.Blazor.Pages
{
    public class DashboardComponent : ComponentBase
    {
        [Parameter] public string IdentityId { get; set; }
        [Parameter] public string Role { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if(string.IsNullOrWhiteSpace(Role) || string.IsNullOrWhiteSpace(IdentityId))
            {
                Snackbar.Add("An error occured while logging in. Please contact support", Severity.Error);
                NavigationManager.NavigateTo("/");
            }
        }


    }
}