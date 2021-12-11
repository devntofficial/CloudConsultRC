namespace CloudConsult.Notification.Events.Consultation
{
    public class ConsultationBaseEvent
    {
        public string EventId { get; set; } = string.Empty;
        public string ConsultationId { get; set; } = string.Empty;
        public string DoctorProfileId { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string DoctorEmailId { get; set; } = string.Empty;
        public string DoctorMobileNo { get; set; } = string.Empty;
        public string MemberProfileId { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public string MemberEmailId { get; set; } = string.Empty;
        public string MemberMobileNo { get; set; } = string.Empty;
    }
}
