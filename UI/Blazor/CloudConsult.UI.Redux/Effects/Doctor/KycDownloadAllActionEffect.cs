using CloudConsult.UI.Interfaces.Doctor;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudConsult.UI.Redux.Effects.Doctor
{
    public class KycDownloadAllActionEffect : Effect<KycDownloadAllAction>
    {
        private readonly IKycService kycService;

        public KycDownloadAllActionEffect(IKycService kycService)
        {
            this.kycService = kycService;
        }

        public override async Task HandleAsync(KycDownloadAllAction action, IDispatcher dispatcher)
        {
            var response = await kycService.DownloadAll(action.ProfileId);
            if(response is null)
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
