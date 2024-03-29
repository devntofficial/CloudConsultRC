﻿using CloudConsult.UI.Interfaces.Doctor;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Doctor
{
    public class GetProfileByIdentityActionEffect : Effect<GetProfileByIdentityAction>
    {
        private readonly IProfileService profileService;

        public GetProfileByIdentityActionEffect(IProfileService profileService)
        {
            this.profileService = profileService;
        }
        public override async Task HandleAsync(GetProfileByIdentityAction action, IDispatcher dispatcher)
        {
            var response = await profileService.GetProfileByIdentityId(action.IdentityId);
            if(response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
            }

            if(!response.IsSuccess)
            {
                dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
            }

            dispatcher.Dispatch(new GetProfileByIdentitySuccessAction(response.Payload.ProfileId, response.Payload));
        }
    }
}
