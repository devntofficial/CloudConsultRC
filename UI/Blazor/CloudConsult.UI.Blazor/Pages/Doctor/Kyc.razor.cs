using CloudConsult.UI.Blazor.Common;
using CloudConsult.UI.Redux.Actions.Doctor;
using CloudConsult.UI.Redux.States.Doctor;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace CloudConsult.UI.Blazor.Pages.Doctor
{
    public class DoctorKycComponent : SessionComponent<KycState>
    {

        protected override async Task OnInitializedAsync()
        {
            Subscriber.SubscribeToAction<KycUploadSuccessAction>(this, action => OnKycUploadSuccess(action));
            Subscriber.SubscribeToAction<GetKycMetadataSuccessAction>(this, action => OnGetKycMetadataSuccess(action));
            Subscriber.SubscribeToAction<GetKycMetadataNotFoundAction>(this, action => OnGetKycMetadataNotFoundAction(action));
            Subscriber.SubscribeToAction<KycDownloadSuccessAction>(this, async action => await OnKycDownloadSuccess(action));

            await base.OnInitializedAsync();

            Dispatcher.Dispatch(new GetKycMetadataAction(ProfileId));
        }

        private async Task OnKycDownloadSuccess(KycDownloadSuccessAction action)
        {
            var file = action.KycDocument;
            await JavaScript.InvokeVoidAsync("DownloadFile", file.FileName, file.FileType, file.FileData);
        }

        private void OnGetKycMetadataNotFoundAction(GetKycMetadataNotFoundAction action)
        {
            Notifier.Add("No KYC documents found", Severity.Info);
        }

        private void OnGetKycMetadataSuccess(GetKycMetadataSuccessAction action)
        {
            
        }

        private void OnKycUploadSuccess(KycUploadSuccessAction action)
        {
            Dispatcher.Dispatch(new GetKycMetadataAction(ProfileId));
            Notifier.Add("Document(s) uploaded successfully");
        }

        protected void OnFileUpload(InputFileChangeEventArgs e)
        {
            Dispatcher.Dispatch(new KycUploadAction(ProfileId, e.GetMultipleFiles().ToList()));
        }

        protected void OnDownloadAllClick()
        {
            Dispatcher.Dispatch(new KycDownloadAllAction(ProfileId));
        }

        protected void OnDownloadFileClick(string fileName)
        {
            Dispatcher.Dispatch(new KycDownloadOneAction(ProfileId, fileName));
        }
    }
}
