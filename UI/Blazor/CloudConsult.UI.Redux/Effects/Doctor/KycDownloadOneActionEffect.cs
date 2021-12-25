using CloudConsult.UI.Interfaces.Doctor;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Doctor
{
    public class KycDownloadOneActionEffect : Effect<KycDownloadOneAction>
    {
        private readonly IKycService kycService;

        public KycDownloadOneActionEffect(IKycService kycService)
        {
            this.kycService = kycService;
        }

        public override async Task HandleAsync(KycDownloadOneAction action, IDispatcher dispatcher)
        {
            var response = await kycService.DownloadOne(action.ProfileId, action.FileName);
            if (response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
                return;
            }

            if (response.IsSuccess)
            {
                dispatcher.Dispatch(new KycDownloadSuccessAction(action.ProfileId, response.Payload));
                return;
            }

            dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
        }
    }
}
