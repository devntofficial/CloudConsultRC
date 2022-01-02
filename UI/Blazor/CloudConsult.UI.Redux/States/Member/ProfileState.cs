using CloudConsult.UI.Data.Member;
using CloudConsult.UI.Redux.States.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.States.Member
{
    [FeatureState]
    public record ProfileState : UIState
    {
        public string ProfileId { get; init; }
        public ProfileData Data { get; init; }
    }
}
