using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace CloudConsult.Doctor.Domain.Entities
{
    public class DoctorProfile
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string IdentityId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AadhaarNo { get; set; } = string.Empty;
        public string Speciality { get; set; } = "General Physician";
        public bool IsActive { get; set; } = false;
        public bool IsCreatedEventPublished { get; set; } = false;
        public bool IsUpdatedEventPublished { get; set; } = false;
        [BsonDateTimeOptions] public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}