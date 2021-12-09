using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Identity.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Identity.Domain.Queries
{
    public record GetToken(string EmailId, string Password) : IQuery<GetTokenResponse>
    {
        public string EmailId { get; set; } = EmailId;
        public string Password { get; set; } = Password;
    }

    public class GetTokenQueryValidator : ApiValidator<GetToken>
    {
        public GetTokenQueryValidator()
        {
            RuleFor(x => x.EmailId).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}