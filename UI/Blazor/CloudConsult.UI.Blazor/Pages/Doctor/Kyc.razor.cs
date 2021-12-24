using CloudConsult.UI.Blazor.Common;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.States.Doctor;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CloudConsult.UI.Blazor.Pages.Doctor
{
    public class DoctorKycComponent : SessionComponent<KycState>
    {

        protected override async Task OnInitializedAsync()
        {
            Subscriber.SubscribeToAction<KycUploadSuccessAction>(this, action => OnKycUploadSuccess(action));
            await base.OnInitializedAsync();
        }

        private void OnKycUploadSuccess(KycUploadSuccessAction action)
        {
            Notifier.Add("Document(s) uploaded successfully");
        }

        protected void OnFileUpload(InputFileChangeEventArgs e)
        {
            Dispatcher.Dispatch(new KycUploadAction(ProfileId, e.GetMultipleFiles().ToList()));
        }
    }
}
