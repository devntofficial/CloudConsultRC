using CloudConsult.UI.Data.Consultation;
using CloudConsult.UI.Redux.States.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.States.Consultation
{
    [FeatureState]
    public record TimeSlotState : UIState
    {
        public List<TimeSlot> TimeSlots { get; init; } = new();
    }
}
