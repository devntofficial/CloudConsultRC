using CloudConsult.UI.Data.Consultation;

namespace CloudConsult.UI.Redux.Actions.Consultation
{
    public class GetTimeSlotsAction
    {
        public string ProfileId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public GetTimeSlotsAction(string ProfileId, DateTime StartDateTime, DateTime EndDateTime)
        {
            this.ProfileId = ProfileId;
            this.StartDateTime = StartDateTime;
            this.EndDateTime = EndDateTime;
        }
    }

    public class GetTimeSlotsSuccessAction
    {
        public List<TimeSlotResponseData> TimeSlots { get; set; }
        public GetTimeSlotsSuccessAction(List<TimeSlotResponseData> TimeSlots)
        {
            this.TimeSlots = TimeSlots;
        }
    }

    public class AddTimeSlotAction
    {
        public string ProfileId { get; set; }
        public DateTime TimeSlotStart { get; set; }
        public DateTime TimeSlotEnd { get; set; }
        public AddTimeSlotAction(string ProfileId, DateTime TimeSlotStart, DateTime TimeSlotEnd)
        {
            this.ProfileId = ProfileId;
            this.TimeSlotStart = TimeSlotStart;
            this.TimeSlotEnd = TimeSlotEnd;
        }
    }

    public class AddTimeSlotSuccessAction
    {
        public TimeSlotResponseData TimeSlot { get; set; }
        public AddTimeSlotSuccessAction(TimeSlotResponseData TimeSlot)
        {
            this.TimeSlot = TimeSlot;
        }
    }
}
