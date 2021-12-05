﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CloudConsult.Doctor.Domain.Entities
{
    public class DoctorKyc
    {
        [BsonId] public ObjectId Id { get; set; } = ObjectId.Empty;
        public ObjectId ProfileId { get; set; } = ObjectId.Empty;
        public bool IsApproved { get; set; } = false;
        public string AdministratorId { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public bool IsEventPublished { get; set; } = false;
        [BsonDateTimeOptions] public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
