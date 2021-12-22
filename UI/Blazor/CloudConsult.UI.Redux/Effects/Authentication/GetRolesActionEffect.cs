using CloudConsult.UI.Interfaces.Identity;
using CloudConsult.UI.Redux.Actions.Authentication;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Authentication
{
    public class GetRolesActionEffect : Effect<GetRolesAction>
    {
        private readonly IIdentityService identityService;

        public GetRolesActionEffect(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        public override async Task HandleAsync(GetRolesAction action, IDispatcher dispatcher)
        {
            var response = await identityService.GetUserRoles();
            if (response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
            }

            if (response.IsSuccess == false)
            {
                dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
                return;
            }

            dispatcher.Dispatch(new GetRolesSuccessAction(response.Payload));
        }
    }
}
