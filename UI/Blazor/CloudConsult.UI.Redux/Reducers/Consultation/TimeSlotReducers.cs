using CloudConsult.UI.Redux.Actions.Consultation;
using CloudConsult.UI.Redux.Actions.Shared;
using CloudConsult.UI.Redux.States.Consultation;
using Fluxor;

namespace CloudConsult.UI.Redux.Reducers.Consultation
{
    public static class TimeSlotReducers
    {
        [ReducerMethod(typeof(GetTimeSlotsAction))]
        public static TimeSlotState OnGetTimeSlotsAction(TimeSlotState state)
        {
            return state with
            {
                Loading = true
            };
        }

        [ReducerMethod]
        public static TimeSlotState OnGetTimeSlotsSuccessAction(TimeSlotState state, GetTimeSlotsSuccessAction action)
        {
            return state with
            {
                Loading = false,
                TimeSlots = action.TimeSlots
            };
        }

        [ReducerMethod]
        public static TimeSlotState OnGatewayErrorAction(TimeSlotState state, GatewayErrorAction action)
        {
            return state with
            {
                Loading = false,
                Processing = false,
                Errors = action.Errors
            };
        }
    }
}
