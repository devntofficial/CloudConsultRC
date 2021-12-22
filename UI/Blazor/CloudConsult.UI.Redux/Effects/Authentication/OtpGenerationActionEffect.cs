using CloudConsult.UI.Interfaces.Identity;
using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Authentication
{
    public class OtpGenerationActionEffect : Effect<OtpGenerationAction>
    {
        private readonly IIdentityService identityService;

        public OtpGenerationActionEffect(IIdentityService identityService)
        {
            this.identityService = identityService;
        }
        public override async Task HandleAsync(OtpGenerationAction action, IDispatcher dispatcher)
        {
            var response = await identityService.GenerateOtp(action.IdentityId);
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

            dispatcher.Dispatch(new OtpGenerationSuccessAction());
        }
    }
}
