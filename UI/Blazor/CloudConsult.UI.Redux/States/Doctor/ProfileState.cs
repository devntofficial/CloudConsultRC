using CloudConsult.UI.Data.Doctor;
using CloudConsult.UI.Redux.States.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.States.Doctor
{
    [FeatureState]
    public record ProfileState : UIState
    {
        public string ProfileId { get; init; }
        public ProfileData Data { get; init; }
    }
}
