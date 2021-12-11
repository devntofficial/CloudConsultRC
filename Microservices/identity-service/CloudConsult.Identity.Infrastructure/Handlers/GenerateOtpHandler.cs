using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Identity.Domain.Commands;
using CloudConsult.Identity.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Identity.Infrastructure.Handlers
{
    public class GenerateOtpHandler : ICommandHandler<GenerateOtp>
    {
        private readonly IApiResponseBuilder builder;
        private readonly ITokenService tokenService;
        private readonly IValidator<GenerateOtp> validator;

        public GenerateOtpHandler(IApiResponseBuilder builder, ITokenService tokenService, IValidator<GenerateOtp> validator)
        {
            this.builder = builder;
            this.tokenService = tokenService;
            this.validator = validator;
        }

        public async Task<IApiResponse> Handle(GenerateOtp request, CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if(validation.Errors.Any())
            {
                return builder.CreateErrorResponse(x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            await tokenService.GenerateOtpFor(request.IdentityId, cancellationToken);
            return builder.CreateSuccessResponse(x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Otp generated successfully");
            });
        }
    }
}
