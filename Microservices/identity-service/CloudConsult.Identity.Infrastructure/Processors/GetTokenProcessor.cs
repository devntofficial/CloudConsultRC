using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Identity.Domain.Queries;
using CloudConsult.Identity.Domain.Responses;
using CloudConsult.Identity.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Identity.Infrastructure.Processors
{
    public class GetTokenProcessor : IQueryProcessor<GetToken, GetTokenResponse>
    {
        private readonly IApiResponseBuilder<GetTokenResponse> builder;
        private readonly IValidator<GetToken> validator;
        private readonly IIdentityService identityService;
        private readonly ITokenService tokenService;

        public GetTokenProcessor(IApiResponseBuilder<GetTokenResponse> builder,
            IValidator<GetToken> validator, IIdentityService identityService,
            ITokenService tokenService)
        {
            this.builder = builder;
            this.validator = validator;
            this.identityService = identityService;
            this.tokenService = tokenService;
        }

        public async Task<IApiResponse<GetTokenResponse>> Handle(GetToken request, CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (validation.Errors.Any())
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var authenticatedUser = await identityService.Authenticate(request, cancellationToken);
            if (authenticatedUser == null)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors("Provided username/password combination is invalid");
                });
            }

            if (authenticatedUser.IsVerified == false)
            {
                await tokenService.GenerateOtpFor(authenticatedUser.Id, cancellationToken);
                return builder.CreateErrorResponse(new GetTokenResponse
                {
                    IdentityId = authenticatedUser.Id,
                    IsVerified = authenticatedUser.IsVerified
                }, x =>
                {
                    x.WithErrorCode(StatusCodes.Status401Unauthorized);
                    x.WithErrors("Your email is not verified. Please validate an OTP sent to your email.");
                });
            }

            if (authenticatedUser.IsBlocked)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors("User was blocked by an administrator. Please contact us for support.");
                });
            }

            var token = tokenService.GenerateJwtTokenFor(authenticatedUser);
            return builder.CreateSuccessResponse(token, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Token generated successfully");
            });
        }
    }
}