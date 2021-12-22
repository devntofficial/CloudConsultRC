using CloudConsult.UI.Interfaces.Doctor;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Doctor
{
    public class UpdateProfileActionEffect : Effect<UpdateProfileAction>
    {
        private readonly IProfileService profileService;

        public UpdateProfileActionEffect(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        public override async Task HandleAsync(UpdateProfileAction action, IDispatcher dispatcher)
        {
            var response = await profileService.UpdateProfile(action.ProfileId, action.Data);
            if(response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
            }

            if(!response.IsSuccess)
            {
                dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
            }

            dispatcher.Dispatch(new UpdateProfileSuccessAction());
        }
    }
}
