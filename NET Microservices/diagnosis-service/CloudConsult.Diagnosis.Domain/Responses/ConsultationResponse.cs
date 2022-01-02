namespace CloudConsult.Diagnosis.Domain.Responses
{
    public class ConsultationResponse
    {
        public string ConsultationId { get; set; } = string.Empty;
        public string DoctorProfileId { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string DoctorEmailId { get; set; } = string.Empty;
        public string DoctorMobileNo { get; set; } = string.Empty;
        public string MemberProfileId { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public string MemberEmailId { get; set; } = string.Empty;
        public string MemberMobileNo { get; set; } = string.Empty;
        public string BookingDate { get; set; } = string.Empty;
        public string BookingTimeSlot { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}
