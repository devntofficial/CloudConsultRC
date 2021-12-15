using Blazored.LocalStorage;
using CloudConsult.UI.Helpers;
using CloudConsult.UI.Models.Identity;
using CloudConsult.UI.Services.Interfaces;
using CloudConsult.UI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Net.Http.Headers;

namespace CloudConsult.UI.Pages.Authentication
{
    public class LoginComponent : ComponentBase
    {
        protected readonly LoginModel LoginModel = new();
        protected InputType PasswordInput = InputType.Password;
        protected string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
        protected bool ShowLoadingSpinner;
        private bool _passwordVisibility;

        [Inject] private IIdentityService IdentityService { get; set; }
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject] private ILocalStorageService LocalStorage { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private HttpClient Client { get; set; }
        [CascadingParameter] private Error Error { get; set; }

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
                var result = await IdentityService.GetToken(LoginModel);
                if(result.IsSuccess)
                {
                    var token = result.Payload.AccessToken;
                    await LocalStorage.SetItemAsync("AccessToken", token);
                    ((AuthStateProvider)AuthStateProvider).NotifyUserLogin(token);
                    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    NavigationManager.NavigateTo("/dashboard");
                }
                else if(result.StatusCode == 401)
                {
                    NavigationManager.NavigateTo($"/verify-otp/{result.Payload.IdentityId}");
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
