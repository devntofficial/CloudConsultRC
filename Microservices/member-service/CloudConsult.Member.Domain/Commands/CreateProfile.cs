using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Member.Domain.Responses;
using FluentValidation;
using System.Text.Json.Serialization;

namespace CloudConsult.Member.Domain.Commands
{
    public class CreateProfile : ICommand<ProfileResponse>
    {
        [JsonIgnore]
        public string IdentityId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
    }

    public class CreateProfileValidator : ApiValidator<CreateProfile>
    {
        public CreateProfileValidator()
        {
            RuleFor(x => x.IdentityId)
            .NotEmpty()
            .Must(BeValidGuid)
            .WithMessage("IdentityId has invalid format");

            RuleFor(x => x.FullName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100)
                .Must(BeAlphabetsOrWhitespaceOnly)
                .WithMessage("FullName contains invalid characters");

            RuleFor(x => x.Gender)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(6)
                .Must(BeValidGender)
                .WithMessage("Gender must be one of 'Male', 'Female', or 'N/A'");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(10)
                .Must(HaveValidDateFormat)
                .WithMessage("DateOfBirth should be in 'dd-mm-yyyy' format");

            RuleFor(x => x.MobileNo)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(10)
                .Must(BeNumericOnly)
                .WithMessage("MobileNo can only contain numbers");

            RuleFor(x => x.EmailId)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(100)
                .EmailAddress();

            RuleFor(x => x.Address)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(200);
        }
    }
}