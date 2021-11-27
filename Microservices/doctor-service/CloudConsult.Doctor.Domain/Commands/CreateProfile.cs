using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Common.CQRS;
using FluentValidation;

namespace CloudConsult.Doctor.Domain.Commands
{
    public record CreateProfile : ICommand<ProfileResponse>
    {
        public Guid IdentityId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
    }

    public class CreateProfileValidator : AbstractValidator<CreateProfile>
    {
        public CreateProfileValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
        }
    }
}