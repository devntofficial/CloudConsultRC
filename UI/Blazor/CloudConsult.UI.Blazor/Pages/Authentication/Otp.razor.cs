using CloudConsult.UI.Blazor.Shared;
using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.States.Authentication;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CloudConsult.UI.Blazor.Pages.Authentication
{
    public class OtpComponent : BaseComponent<OtpState>
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
        [Parameter] public string IdentityId { get; set; }
        [CascadingParameter] private Error Error { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Dispatcher.Dispatch(new OtpGenerationAction(IdentityId));
            Subscriber.SubscribeToAction<OtpVerificationSuccessAction>(this, action => OnOtpVerificationSuccess(action));
            await base.OnInitializedAsync();
        }

        protected void VerifyOtpButtonClick()
        {
            Dispatcher.Dispatch(new OtpVerificationAction(IdentityId, Convert.ToInt32($"{num1}{num2}{num3}{num4}{num5}{num6}")));
        }

        private void OnOtpVerificationSuccess(OtpVerificationSuccessAction action)
        {
            Notifier.Add("Your account was registered", Severity.Info);
            Notifier.Add("Please login with your chosen password to proceed", Severity.Info);
            Navigation.NavigateTo("/");
        }

        protected void ResendOtpLinkClick()
        {
            Dispatcher.Dispatch(new OtpGenerationAction(IdentityId));
        }

        protected bool isOtpFilled()
        {
            return new[] { num1, num2, num3, num4, num5, num6 }.Any(x => x == null);
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
