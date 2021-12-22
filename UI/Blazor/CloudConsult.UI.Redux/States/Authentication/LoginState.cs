using CloudConsult.UI.Data.Authentication;
using CloudConsult.UI.Redux.States.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.States.Authentication
{
    [FeatureState]
    public record LoginState : UIState
    {
        public string IdentityId { get; init; } = string.Empty;
        public LoginData Data { get; init; } = new();
    }
}
