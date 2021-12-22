using CloudConsult.UI.Data.Doctor;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.States.Doctor;
using Microsoft.AspNetCore.Components;

namespace CloudConsult.UI.Blazor.Pages.Doctor
{
    public class CreateProfileComponent : BaseComponent<ProfileState>
    {
        protected ProfileData data = new();
        protected DateTime? dateOfBirthPicker;
        [Parameter] public string IdentityId { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            data.IdentityId = IdentityId;
            data.FullName = await SessionStorage.GetItemAsync<string>("FullName");
            data.EmailId = await SessionStorage.GetItemAsync<string>("EmailId");
            await SessionStorage.ClearAsync();
            Subscriber.SubscribeToAction<CreateProfileSuccessAction>(this, action => OnCreateProfileSuccess(action));
            await base.OnInitializedAsync();
        }

        private void OnCreateProfileSuccess(CreateProfileSuccessAction action)
        {
            Navigation.NavigateTo($"/verify-otp/{data.IdentityId}");
        }

        public void CreateProfileClick()
        {
            data.DateOfBirth = dateOfBirthPicker.HasValue ? dateOfBirthPicker.Value.ToString("dd-MM-yyyy") : string.Empty;
            Dispatcher.Dispatch(new CreateProfileAction(data));
        }
    }
}
