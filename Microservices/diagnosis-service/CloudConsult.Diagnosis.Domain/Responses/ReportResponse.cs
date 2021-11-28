using CloudConsult.Diagnosis.Domain.Commands;

namespace CloudConsult.Diagnosis.Domain.Responses
{
    public class ReportResponse
    {
        public string ReportId { get; set; } = "";
        public string ConsultationId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Summary { get; set; } = "";
        public List<MedicinePrescription> MedicinePrescriptions { get; set; } = new();
    }
}
