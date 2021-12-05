using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Identity.Domain.Commands;
using CloudConsult.Identity.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Identity.Infrastructure.Handlers
{
    public class ValidateOtpHandler : ICommandHandler<ValidateOtp>
    {
        private readonly IApiResponseBuilder builder;
        private readonly ITokenService tokenService;
        private readonly IValidator<ValidateOtp> validator;

        public ValidateOtpHandler(IApiResponseBuilder builder, ITokenService tokenService, IValidator<ValidateOtp> validator)
        {
            this.builder = builder;
            this.tokenService = tokenService;
            this.validator = validator;
        }

        public async Task<IApiResponse> Handle(ValidateOtp request, CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (validation.Errors.Any())
            {
                return builder.CreateErrorResponse(x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var isValidOtp = await tokenService.ValidateOtp(Guid.Parse(request.IdentityId), request.Otp, cancellationToken);
            if(!isValidOtp)
            {
                return builder.CreateErrorResponse(x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors("Invalid otp");
                });
            }

            return builder.CreateSuccessResponse(x =>
            {
                x.WithSuccessCode(StatusCodes.Status204NoContent);
                x.WithMessages("Otp is valid");
            });
        }
    }
}
