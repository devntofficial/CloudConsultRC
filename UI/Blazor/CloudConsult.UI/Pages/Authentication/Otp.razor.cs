using CloudConsult.UI.Services.Interfaces;
using CloudConsult.UI.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace CloudConsult.UI.Pages.Authentication
{
    public class OtpComponent : ComponentBase
    {
        protected int? num1 = null;
        protected int? num2 = null;
        protected int? num3 = null;
        protected int? num4 = null;
        protected int? num5 = null;
        protected int? num6 = null;
        protected MudTextField<int?> num1Ref;
        protected MudTextField<int?> num2Ref;
        protected MudTextField<int?> num3Ref;
        protected MudTextField<int?> num4Ref;
        protected MudTextField<int?> num5Ref;
        protected MudTextField<int?> num6Ref;
        protected bool ShowLoadingSpinner;
        [Parameter] public string IdentityId { get; set; }
        [Inject] private IIdentityService IdentityService { get; set; }
        [Inject] private NavigationManager Navigation { get; set; }
        [Inject] private ISnackbar Snackbar { get; set; }
        [CascadingParameter] private Error Error { get; set; }

        protected async Task GenerateOtp()
        {
            var output = await IdentityService.GenerateOtp(IdentityId);
            if (output.IsSuccess)
            {
                Snackbar.Add("OTP was sent successfully", Severity.Info);
            }
            else
            {
                for (int i = 0; i < output.Errors.Count(); i++)
                {
                    Snackbar.Add(output.Errors.ElementAt(i), Severity.Error);
                }
            }
        }

        protected bool isOtpFilled()
        {
            return new[] { num1, num2, num3, num4, num5, num6 }.Any(x => x == null);
        }

        protected async Task ValidateOtp()
        {
            ShowLoadingSpinner = true;
            try
            {
                var output = await IdentityService.ValidateOtp(IdentityId, Convert.ToInt32($"{num1}{num2}{num3}{num4}{num5}{num6}"));
                if (output is not null)
                {
                    if (output.IsSuccess)
                    {
                        Snackbar.Add("OTP verified successfully. Please login again to proceed.", Severity.Info);
                        Navigation.NavigateTo("/");
                    }
                    else
                    {
                        for (int i = 0; i < output.Errors.Count(); i++)
                        {
                            Snackbar.Add(output.Errors.ElementAt(i), Severity.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
            }
            ShowLoadingSpinner = false;
        }

        protected void GoBackToLogin()
        {
            Navigation.NavigateTo("/");
        }

        protected async Task focusToNum2(string num)
        {
            if (num1 is not null) await num2Ref.FocusAsync();
        }
        protected async Task focusToNum3(string num)
        {
            if (num2 is not null) await num3Ref.FocusAsync();
        }
        protected async Task focusToNum4(string num)
        {
            if (num3 is not null) await num4Ref.FocusAsync();
        }
        protected async Task focusToNum5(string num)
        {
            if (num4 is not null) await num5Ref.FocusAsync();
        }
        protected async Task focusToNum6(string num)
        {
            if (num5 is not null) await num6Ref.FocusAsync();
        }
    }
}
