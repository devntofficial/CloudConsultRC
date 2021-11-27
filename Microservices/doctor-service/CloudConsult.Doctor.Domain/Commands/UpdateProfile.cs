using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Responses;
using FluentValidation;
using System.Text.Json.Serialization;

namespace CloudConsult.Doctor.Domain.Commands
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

    public class UpdateProfileValidator : AbstractValidator<UpdateProfile>
    {
        public UpdateProfileValidator()
        {

        }
    }
}
