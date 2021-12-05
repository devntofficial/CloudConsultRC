using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CloudConsult.Doctor.Domain.Entities
{
    public class DoctorProfile
    {
        [BsonId] public ObjectId Id { get; set; } = ObjectId.Empty;
        public string IdentityId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty ;
        public string EmailId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AadhaarNo { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public bool IsCreatedEventPublished { get; set; } = false;
        public bool IsUpdatedEventPublished { get; set; } = false;
        [BsonDateTimeOptions] public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}