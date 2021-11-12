using CloudConsult.Common.CQRS;
using CloudConsult.Identity.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Identity.Domain.Queries
{
    public record GetTokenQuery(string EmailId, string Password) : IQuery<GetTokenResponse>
    {
        public string EmailId { get; set; } = EmailId;
        public string Password { get; set; } = Password;
    }

    public class GetTokenQueryValidator : AbstractValidator<GetTokenQuery>
    {
        public GetTokenQueryValidator()
        {
            RuleFor(x => x.EmailId).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        }
    }
}