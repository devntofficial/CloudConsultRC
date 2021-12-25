namespace CloudConsult.UI.Data.Consultation
{
    public class TimeSlotRangeResponseData
    {
        public string ProfileId { get; set; }
        public List<TimeSlot> TimeSlots { get; set; }
    }

    public class TimeSlot
    {
        public string Id { get; set; }
        public DateTime TimeSlotStart { get; set; }
        public DateTime TimeSlotEnd { get; set; }
        public bool IsBooked { get; set; }
        public string ConsultationId { get; set; }
        public string MemberName { get; set; }
        public string DisplayText { get; set; }
    }
}
