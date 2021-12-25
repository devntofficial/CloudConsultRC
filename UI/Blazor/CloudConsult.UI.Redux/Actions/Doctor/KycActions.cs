using CloudConsult.UI.Data.Doctor;
using Microsoft.AspNetCore.Components.Forms;

namespace CloudConsult.UI.Redux.Actions.Doctor
{
    public class KycUploadAction
    {
        public List<IBrowserFile> Files { get; }
        public string ProfileId { get; set; }
        public KycUploadAction(string ProfileId, List<IBrowserFile> Files)
        {
            this.ProfileId = ProfileId;
            this.Files = Files;
        }
    }

    public class KycUploadSuccessAction { }

    public class GetKycMetadataAction
    {
        public string ProfileId { get; set; }
        public GetKycMetadataAction(string ProfileId)
        {
            this.ProfileId = ProfileId;
        }
    }

    public class GetKycMetadataSuccessAction
    {
        public List<KycMetadata> KycDocumentsMetadata { get; set; }
        public GetKycMetadataSuccessAction(List<KycMetadata> KycDocumentsMetadata)
        {
            this.KycDocumentsMetadata = KycDocumentsMetadata;
        }
    }

    public class GetKycMetadataNotFoundAction { }

    public class KycDownloadAllAction
    {
        public string ProfileId { get; }
        public KycDownloadAllAction(string ProfileId)
        {
            this.ProfileId = ProfileId;
        }
    }

    public class KycDownloadSuccessAction
    {
        public string ProfileId { get; }
        public KycDocumentResponseData KycDocument { get; set; }
        public KycDownloadSuccessAction(string ProfileId, KycDocumentResponseData KycDocument)
        {
            this.ProfileId = ProfileId;
            this.KycDocument = KycDocument;
        }
    }

    public class KycDownloadOneAction
    {
        public string ProfileId { get; }
        public string FileName { get; }
        public KycDownloadOneAction(string ProfileId, string FileName)
        {
            this.ProfileId = ProfileId;
            this.FileName = FileName;
        }
    }
}
