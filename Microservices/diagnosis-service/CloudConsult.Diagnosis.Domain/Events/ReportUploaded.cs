using CloudConsult.Diagnosis.Domain.Commands;
using MongoDB.Bson;

namespace CloudConsult.Diagnosis.Domain.Events
{
    public class ReportUploaded
    {
        public ObjectId ReportId { get; set; } = ObjectId.Empty;
        public string ConsultationId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<MedicinePrescription> MedicinePrescriptions { get; set; } = new();
    }
}
