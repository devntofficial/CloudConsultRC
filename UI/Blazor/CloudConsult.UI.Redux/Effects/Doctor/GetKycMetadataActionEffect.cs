using CloudConsult.UI.Interfaces.Doctor;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Doctor
{
    public class GetKycMetadataActionEffect : Effect<GetKycMetadataAction>
    {
        private readonly IKycService kycService;
        public GetKycMetadataActionEffect(IKycService kycService)
        {
            this.kycService = kycService;
        }
        public override async Task HandleAsync(GetKycMetadataAction action, IDispatcher dispatcher)
        {
            var response = await kycService.GetMetadata(action.ProfileId);
            if(response.IsSuccess)
            {
                dispatcher.Dispatch(new GetKycMetadataSuccessAction(response.Payload.KycDocumentsMetadata));
                return;
            }

            if(response.StatusCode == 404)
            {
                dispatcher.Dispatch(new GetKycMetadataNotFoundAction());
                return;
            }

            dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
        }
    }
}
