using CloudConsult.UI.Interfaces.Member;
using CloudConsult.UI.Redux.Actions.Member;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Member
{
    public class CreateProfileActionEffect : Effect<CreateProfileAction>
    {
        private readonly IProfileService profileService;

        public CreateProfileActionEffect(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        public override async Task HandleAsync(CreateProfileAction action, IDispatcher dispatcher)
        {
            var response = await profileService.CreateProfile(action.Data);
            if (response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
            }

            if (response.IsSuccess == false)
            {
                dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
                return;
            }

            dispatcher.Dispatch(new CreateProfileSuccessAction(response.Payload.ProfileId));
        }
    }
}
