using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CloudConsult.Doctor.Domain.Entities
{
    public class DoctorProfile
    {
        [BsonId] public ObjectId Id { get; set; }
        public string IdentityId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
        public bool IsActive { get; set; }
        public bool ProfileCreatedEventPublished { get; set; }
        public bool ProfileUpdatedEventPublished { get; set; }

        [BsonDateTimeOptions] public DateTime CreatedDate { get; set; }
    }
}