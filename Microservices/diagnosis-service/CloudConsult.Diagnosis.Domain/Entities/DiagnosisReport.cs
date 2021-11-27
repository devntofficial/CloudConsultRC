using CloudConsult.Diagnosis.Domain.Commands;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CloudConsult.Diagnosis.Domain.Entities
{
    public class DiagnosisReport
    {
        [BsonId] public ObjectId Id { get; set; }
        public string ConsultationId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Summary { get; set; } = "";
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public List<MedicinePrescription> MedicinePrescriptions { get; set; } = new();
    }
}
