namespace CloudConsult.Consultation.Domain.Responses
{
    public class ConsultationResponse
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public List<ConsultationData> Consultations { get; set; } = new();
    }

    public class ConsultationData
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string BookingDate { get; set; }
        public string BookingTimeSlot { get; set; }
        public string Status { get; set; }
        public bool IsAcceptedByDoctor { get; set; }
        public bool IsPaymentComplete { get; set; }
        public bool IsDiagnosisReportGenerated { get; set; }
        public string DiagnosisReportId { get; set; }
        public bool IsConsultationComplete { get; set; }
    }
}
