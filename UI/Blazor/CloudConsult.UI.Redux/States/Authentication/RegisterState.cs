using CloudConsult.UI.Data.Authentication;
using CloudConsult.UI.Redux.States.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.States.Authentication
{
    [FeatureState]
    public record RegisterState : UIState
    {
        public string IdentityId { get; init; } = string.Empty;
        public RegisterData Data { get; init; } = new();
        public List<RoleData> Roles { get; init; } = new();
    }
}
