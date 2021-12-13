namespace CloudConsult.Consultation.Domain.Events
{
    public class ReportUploaded
    {
        public string ReportId { get; set; } = string.Empty;
        public string ConsultationId { get; set; } = string.Empty;
    }
}
