using CloudConsult.Common.CQRS;
using CloudConsult.Member.Domain.Responses;
using FluentValidation;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace CloudConsult.Member.Domain.Commands
{
    public class UpdateProfile : ICommand<ProfileResponse>
    {
        [JsonIgnore]
        public ObjectId ProfileId { get; set; }
        public Guid IdentityId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
    }

    public class UpdateProfileValidator : AbstractValidator<UpdateProfile>
    {
        public UpdateProfileValidator()
        {

        }
    }
}
