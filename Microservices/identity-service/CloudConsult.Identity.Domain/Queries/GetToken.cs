using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Identity.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Identity.Domain.Queries
{
    public class GetToken : IQuery<GetTokenResponse>
    {
        public string EmailId { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
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