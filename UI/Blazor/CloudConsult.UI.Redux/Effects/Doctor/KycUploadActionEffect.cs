using CloudConsult.UI.Interfaces.Doctor;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Doctor
{
    public class KycUploadActionEffect : Effect<KycUploadAction>
    {
        private readonly IKycService kycService;

        public KycUploadActionEffect(IKycService kycService)
        {
            this.kycService = kycService;
        }

        public override async Task HandleAsync(KycUploadAction action, IDispatcher dispatcher)
        {
            var response = await kycService.Upload(action.ProfileId, action.Files);
            if(response.IsSuccess)
            {
                dispatcher.Dispatch(new KycUploadSuccessAction());
                return;
            }
            dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
        }
    }
}
