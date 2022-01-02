using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using FluentValidation;

namespace CloudConsult.Identity.Domain.Commands
{
    public class GenerateOtp : ICommand
    {
        public string IdentityId { get; set; }
    }

    public class GenerateOtpValidator : ApiValidator<GenerateOtp>
    {
        public GenerateOtpValidator()
        {
            RuleFor(x => x.IdentityId).NotEmpty();
            RuleFor(x => x.IdentityId).Must(BeValidGuid).WithMessage("'IdentityId' has incorrect format");
        }
    }
}
