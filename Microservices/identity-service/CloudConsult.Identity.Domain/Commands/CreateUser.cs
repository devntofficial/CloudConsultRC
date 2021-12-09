using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Identity.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Identity.Domain.Commands
{
    public record CreateUser : ICommand<CreateUserResponse>
    {
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

    public class CreateUserValidator : ApiValidator<CreateUser>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.FullName).NotEmpty();
            RuleFor(x => x.FullName).Must(BeAlphabetsOrWhitespaceOnly).WithMessage("FullName cannot have numbers or symbols");
            RuleFor(x => x.EmailId).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.RoleId).NotEqual(0);
        }
    }
}