using CloudConsult.UI.Redux.States.Shared;
using Fluxor;
using Microsoft.AspNetCore.Components.Forms;

namespace CloudConsult.UI.Redux.States.Doctor
{
    [FeatureState]
    public record KycState : UIState
    {
        public List<IBrowserFile> KycDocuments { get; init; }
    }
}
