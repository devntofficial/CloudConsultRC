using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using FluentValidation;

namespace CloudConsult.Identity.Domain.Commands
{
    public class ValidateOtp : ICommand
    {
        public string IdentityId { get; set; } = string.Empty;
        public int Otp { get; set; } = 0;
    }

    public class ValidateOtpValidator : ApiValidator<ValidateOtp>
    {
        public ValidateOtpValidator()
        {
            RuleFor(x => x.IdentityId).NotEmpty();
            RuleFor(x => x.IdentityId).Must(BeValidGuid).WithMessage("'IdentityId' has incorrect format");
            RuleFor(x => x.Otp).NotEmpty().InclusiveBetween(100000, 999999);
        }
    }
}
