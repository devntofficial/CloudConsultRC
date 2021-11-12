using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CloudConsult.Doctor.Domain.Entities
{
    public class DoctorEntity
    {
        [BsonId] public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
        public bool IsActive { get; set; }
        public bool IsRegistrationEventPublished { get; set; }
        public bool IsUpdationEventPublished { get; set; }

        [BsonDateTimeOptions] public DateTime CreatedDate { get; set; }
    }
}