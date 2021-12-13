using CloudConsult.Diagnosis.Domain.Commands;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CloudConsult.Diagnosis.Domain.Entities
{
    public class DiagnosisReport
    {
        [BsonId] public string Id { get; set; } = string.Empty;
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
        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public bool IsEventPublished { get; set; } = false;
        public List<MedicinePrescription> MedicinePrescriptions { get; set; } = new();
    }
}
