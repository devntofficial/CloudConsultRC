using CloudConsult.UI.Data.Doctor;
using CloudConsult.UI.Redux.States.Shared;
using Fluxor;
using Microsoft.AspNetCore.Components.Forms;

namespace CloudConsult.UI.Redux.States.Doctor
{
    [FeatureState]
    public record KycState : UIState
    {
        public bool Downloading { get; set; }
        public List<IBrowserFile> KycDocuments { get; init; }
        public List<KycMetadata> KycDocumentsMetadata { get; init; }
    }
}
