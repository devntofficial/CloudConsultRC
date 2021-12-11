using CloudConsult.Diagnosis.Domain.Commands;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CloudConsult.Diagnosis.Domain.Entities
{
    public class DiagnosisReport
    {
        [BsonId] public ObjectId Id { get; set; }
        public string ConsultationId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public bool IsEventPublished { get; set; } = false;
        public List<MedicinePrescription> MedicinePrescriptions { get; set; } = new();
    }
}
