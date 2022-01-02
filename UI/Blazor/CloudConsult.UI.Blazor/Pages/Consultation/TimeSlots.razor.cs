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
        protected RadzenScheduler<TimeSlotResponseData> scheduler;
        protected DateTime? datePicker = DateTime.Now;
        protected TimeSpan? startTimePicker = DateTime.Now.RoundUp(TimeSpan.FromHours(1)).TimeOfDay;
        protected TimeSpan? endTimePicker = DateTime.Now.RoundUp(TimeSpan.FromHours(1)).AddHours(1).TimeOfDay;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Subscriber.SubscribeToAction<AddTimeSlotSuccessAction>(this, action => OnAddTimeSlotSuccess(action));
        }

        private void OnAddTimeSlotSuccess(AddTimeSlotSuccessAction action)
        {
            scheduler.Reload();
        }

        protected async Task OnLoadData(SchedulerLoadDataEventArgs args)
        {
            Dispatcher.Dispatch(new GetTimeSlotsAction(await SessionStorage.GetItemAsync<string>("ProfileId"), args.Start, args.End));
        }

        protected void OnAddTimeSlotClick()
        {
            var timeSlotStart = datePicker.Value.Date + startTimePicker.Value;
            var timeSlotEnd = datePicker.Value.Date + endTimePicker.Value;
            Dispatcher.Dispatch(new AddTimeSlotAction(ProfileId, timeSlotStart, timeSlotEnd));
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

        protected async Task OnAppointmentSelect(SchedulerAppointmentSelectEventArgs<TimeSlotResponseData> args)
        {
            //await DialogService.OpenAsync<EditAppointmentPage>("Edit Appointment", new Dictionary<string, object> { { "Appointment", args.Data } });
            await scheduler.Reload();
        }

        protected void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<TimeSlotResponseData> args)
        {
            // Never call StateHasChanged in AppointmentRender - would lead to infinite loop
            if(args.Data.TimeSlotEnd < DateTime.Now)
            {
                args.Attributes["style"] = "background:#00000089;color:white;";
            }
            else if (args.Data.IsBooked)
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
