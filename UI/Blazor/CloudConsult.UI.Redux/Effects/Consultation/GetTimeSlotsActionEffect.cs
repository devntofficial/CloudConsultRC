using CloudConsult.UI.Interfaces.Consultation;
using CloudConsult.UI.Redux.Actions.Consultation;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Consultation
{
    public class GetTimeSlotsActionEffect : Effect<GetTimeSlotsAction>
    {
        private readonly ITimeSlotService timeSlotService;

        public GetTimeSlotsActionEffect(ITimeSlotService timeSlotService)
        {
            this.timeSlotService = timeSlotService;
        }

        public override async Task HandleAsync(GetTimeSlotsAction action, IDispatcher dispatcher)
        {
            var response = await timeSlotService.GetTimeSlots(action.ProfileId, action.StartDateTime, action.EndDateTime);
            if (response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
                return;
            }

            if (response.IsSuccess)
            {
                dispatcher.Dispatch(new GetTimeSlotsSuccessAction(response.Payload.TimeSlots));
                return;
            }

            if (response.StatusCode == 404)
            {
                return;
            }

            dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
        }
    }
}
