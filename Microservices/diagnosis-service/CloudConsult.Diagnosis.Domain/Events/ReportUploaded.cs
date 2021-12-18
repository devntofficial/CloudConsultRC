using CloudConsult.Diagnosis.Domain.Commands;

namespace CloudConsult.Diagnosis.Domain.Events
{
    public class ReportUploaded
    {
        public string ReportId { get; set; } = string.Empty;
        public string ConsultationId { get; set; } = string.Empty;
        public string DoctorProfileId { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public string DoctorEmailId { get; set; } = string.Empty;
        public string DoctorMobileNo { get; set; } = string.Empty;
        public string MemberProfileId { get; set; } = string.Empty;
        public string MemberName { get; set; } = string.Empty;
        public string MemberEmailId { get; set; } = string.Empty;
        public string MemberMobileNo { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<MedicinePrescription> MedicinePrescriptions { get; set; } = new();
    }
}
