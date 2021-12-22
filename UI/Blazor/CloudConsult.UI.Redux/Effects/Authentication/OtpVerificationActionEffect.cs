using CloudConsult.UI.Interfaces.Identity;
using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Authentication
{
    public class OtpVerificationActionEffect : Effect<OtpVerificationAction>
    {
        private readonly IIdentityService identityService;

        public OtpVerificationActionEffect(IIdentityService identityService)
        {
            this.identityService = identityService;
        }
        public override async Task HandleAsync(OtpVerificationAction action, IDispatcher dispatcher)
        {
            var response = await identityService.ValidateOtp(action.IdentityId, action.Otp);
            if (response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
                return;
            }

            if (response.IsSuccess == false)
            {
                dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
                return;
            }

            dispatcher.Dispatch(new OtpVerificationSuccessAction());
        }
    }
}
