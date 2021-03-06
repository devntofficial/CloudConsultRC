using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace CloudConsult.Doctor.Domain.Entities
{
    public class DoctorKyc
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string ProfileId { get; set; } = string.Empty;
        public string EmailId { get; set; }
        public string FullName { get; set; }
        public bool IsApproved { get; set; } = false;
        public string AdministratorId { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public bool IsEventPublished { get; set; } = false;
        [BsonDateTimeOptions] public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
