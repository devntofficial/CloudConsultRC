using CloudConsult.UI.Blazor.Common;
using CloudConsult.UI.Data.Consultation;
using CloudConsult.UI.Redux.Actions.Consultation;
using CloudConsult.UI.Redux.States.Consultation;
using Radzen;
using Radzen.Blazor;

namespace CloudConsult.UI.Blazor.Pages.Consultation
{
    public class TimeSlotsComponent : SessionComponent<TimeSlotState>
    {
        protected RadzenScheduler<TimeSlot> scheduler;
        protected List<TimeSlot> timeSlots = new();

        protected override async Task OnInitializedAsync()
        {
            Subscriber.SubscribeToAction<GetTimeSlotsSuccessAction>(this, action => OnGetTimeSlotsSuccess(action));
            await base.OnInitializedAsync();
        }

        private void OnGetTimeSlotsSuccess(GetTimeSlotsSuccessAction action)
        {
            timeSlots = action.TimeSlots;
        }

        protected async Task OnLoadData(SchedulerLoadDataEventArgs args)
        {
            Dispatcher.Dispatch(new GetTimeSlotsAction(await SessionStorage.GetItemAsync<string>("ProfileId"), args.Start, args.End));
        }

        protected void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            // Highlight today in month view
            if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
            {
                args.Attributes["style"] = "background:#ebf7ec";
            }

            // Highlight working hours (9-18)
            if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour > 8 && args.Start.Hour < 19)
            {
                args.Attributes["style"] = "background:#ebf7ec";
            }
        }

        protected async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {


            //Availability data = await DialogService.OpenAsync<AddAppointmentPage>("Add Appointment",
            //    new Dictionary<string, object> { { "Start", args.Start }, { "End", args.End } });

            //if (data != null)
            //{
            //    appointments.Add(data);
            //    // Either call the Reload method or reassign the Data property of the Scheduler
            //}
            await scheduler.Reload();
        }

        protected async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<TimeSlot> args)
        {
            //await DialogService.OpenAsync<EditAppointmentPage>("Edit Appointment", new Dictionary<string, object> { { "Appointment", args.Data } });
            await scheduler.Reload();
        }

        protected void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<TimeSlot> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop

            if (args.Data.IsBooked)
            {
                args.Attributes["style"] = "background:#43a047";
            }
            else
            {
                args.Attributes["style"] = "background:#43a047";
            }
        }
    }
}
