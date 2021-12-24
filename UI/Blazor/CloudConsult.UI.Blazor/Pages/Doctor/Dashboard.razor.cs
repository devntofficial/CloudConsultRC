using CloudConsult.UI.Blazor.Common;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.States.Doctor;
using Microsoft.AspNetCore.Components;

namespace CloudConsult.UI.Blazor.Pages
{
    public class DoctorDashboardComponent : BaseComponent<DashboardState>
    {
        [Parameter] public string IdentityId { get; set; }

        protected override Task OnInitializedAsync()
        {
            Subscriber.SubscribeToAction<GetProfileByIdentitySuccessAction>(this, action => OnGetProfileSuccess(action));
            Dispatcher.Dispatch(new GetProfileByIdentityAction(IdentityId));
            return base.OnInitializedAsync();
        }

        private void OnGetProfileSuccess(GetProfileByIdentitySuccessAction action)
        {
            SessionStorage.SetItemAsync("ProfileId", action.ProfileId);
        }
    }
}