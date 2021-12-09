using CloudConsult.Diagnosis.Domain.Commands;

namespace CloudConsult.Diagnosis.Domain.Responses
{
    public class ReportResponse
    {
        public string ReportId { get; set; } = string.Empty;
        public string ConsultationId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<MedicinePrescription> MedicinePrescriptions { get; set; } = new();
    }
}
