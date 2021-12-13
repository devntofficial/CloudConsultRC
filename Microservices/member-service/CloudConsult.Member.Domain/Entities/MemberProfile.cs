using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace CloudConsult.Member.Domain.Entities
{
    public class MemberProfile
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string IdentityId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public bool IsCreatedEventPublished { get; set; } = false;
        public bool IsUpdatedEventPublished { get; set; } = false;

        [BsonDateTimeOptions] public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}