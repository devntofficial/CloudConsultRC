using CloudConsult.UI.Interfaces.Consultation;
using CloudConsult.UI.Redux.Actions.Consultation;
using CloudConsult.UI.Redux.Actions.Shared;
using Fluxor;

namespace CloudConsult.UI.Redux.Effects.Consultation
{
    public class AddTimeSlotActionEffect : Effect<AddTimeSlotAction>
    {
        private readonly ITimeSlotService timeSlotService;

        public AddTimeSlotActionEffect(ITimeSlotService timeSlotService)
        {
            this.timeSlotService = timeSlotService;
        }

        public override async Task HandleAsync(AddTimeSlotAction action, IDispatcher dispatcher)
        {
            var response = await timeSlotService.AddTimeSlot(action.ProfileId, action.TimeSlotStart, action.TimeSlotEnd);
            if (response is null)
            {
                dispatcher.Dispatch(new GatewayErrorAction(null));
                return;
            }

            if (response.IsSuccess)
            {
                dispatcher.Dispatch(new AddTimeSlotSuccessAction(response.Payload));
                return;
            }

            dispatcher.Dispatch(new GatewayErrorAction(response.Errors));
        }
    }
}
