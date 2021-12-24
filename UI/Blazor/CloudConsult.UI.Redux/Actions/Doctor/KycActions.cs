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
}
