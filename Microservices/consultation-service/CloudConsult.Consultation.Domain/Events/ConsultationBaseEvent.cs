namespace CloudConsult.Consultation.Domain.Events
{
    public class ConsultationBaseEvent
    {
        public string EventId { get; set; }
        public string ConsultationId { get; set; }
        public string DoctorProfileId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorEmailId { get; set; }
        public string DoctorMobileNo { get; set; }
        public string MemberProfileId { get; set; }
        public string MemberName { get; set; }
        public string MemberEmailId { get; set; }
        public string MemberMobileNo { get; set; }
    }
}
