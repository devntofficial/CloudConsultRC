using CloudConsult.UI.Redux.States.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.States.Authentication
{
    [FeatureState]
    public record OtpState : UIState
    {
        public string IdentityId { get; init; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}
