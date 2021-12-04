﻿using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Member.Domain.Responses;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace CloudConsult.Member.Domain.Commands
{
    public class UpdateProfile : ICommand<ProfileResponse>
    {
        [JsonIgnore]
        public string ProfileId { get; set; }
        public string IdentityId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
    }

    public class UpdateProfileValidator : ApiValidator<UpdateProfile>
    {
        public UpdateProfileValidator()
        {

        }
    }
}
