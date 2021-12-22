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
    public class GetProfileActionEffect : Effect<GetProfileAction>
    {
        private readonly IProfileService profileService;

        public GetProfileActionEffect(IProfileService profileService)
        {
            this.profileService = profileService;
        }
        public override async Task HandleAsync(GetProfileAction action, IDispatcher dispatcher)
        {
            var response = await profileService.GetProfileById(action.ProfileId);
            if (response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
            }

            if (!response.IsSuccess)
            {
                dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
            }

            dispatcher.Dispatch(new GetProfileSuccessAction(response.Payload.ProfileId, response.Payload));
        }
    }
}
