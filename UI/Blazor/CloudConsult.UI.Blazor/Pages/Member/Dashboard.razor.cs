using CloudConsult.UI.Blazor.Common;
using CloudConsult.UI.Redux.Actions.Member;
using CloudConsult.UI.Redux.States.Member;

namespace CloudConsult.UI.Blazor.Pages.Member
{
    public class MemberDashboardComponent : SessionComponent<DashboardState>
    {
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Subscriber.SubscribeToAction<GetProfileByIdentitySuccessAction>(this, action => OnGetProfileSuccess(action));
            Dispatcher.Dispatch(new GetProfileByIdentityAction(IdentityId));
        }

        private void OnGetProfileSuccess(GetProfileByIdentitySuccessAction action)
        {
            SessionStorage.SetItemAsync("ProfileId", action.ProfileId);
        }
    }
}
