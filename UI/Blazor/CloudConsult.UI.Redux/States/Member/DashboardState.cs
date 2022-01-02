using CloudConsult.UI.Redux.States.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.States.Member
{
    [FeatureState]
    public record DashboardState : UIState
    {
        public string ProfileId { get; init; }
    }
}
