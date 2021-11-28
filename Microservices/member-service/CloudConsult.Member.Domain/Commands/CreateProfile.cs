using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Member.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Member.Domain.Commands
{
    public record CreateProfile : ICommand<ProfileResponse>
    {
        public string IdentityId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
    }

    public class CreateProfileValidator : ApiValidator<CreateProfile>
    {
        public CreateProfileValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
        }
    }
}