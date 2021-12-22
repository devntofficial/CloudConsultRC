using CloudConsult.UI.Redux.States.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.States.Doctor
{
    [FeatureState]
    public record DashboardState : UIState
    {
        public string ProfileId { get; init; }
    }
}
