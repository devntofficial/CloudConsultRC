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
        public List<TimeSlot> TimeSlots { get; set; }
        public GetTimeSlotsSuccessAction(List<TimeSlot> TimeSlots)
        {
            this.TimeSlots = TimeSlots;
        }
    }
}
