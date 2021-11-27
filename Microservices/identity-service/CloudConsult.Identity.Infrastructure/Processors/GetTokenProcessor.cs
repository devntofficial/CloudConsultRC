using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IApiResponseBuilder<GetTokenResponse> _builder;
        private readonly IValidator<GetToken> _validator;
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public GetTokenProcessor(IApiResponseBuilder<GetTokenResponse> builder,
            IValidator<GetToken> validator, IIdentityService identityService,
            ITokenService tokenService)
        {
            _builder = builder;
            _validator = validator;
            _identityService = identityService;
            _tokenService = tokenService;
        }
        
        public async Task<IApiResponse<GetTokenResponse>> Handle(GetToken request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (validation.Errors.Any())
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var authenticatedUser = await _identityService.Authenticate(request, cancellationToken);
            if (authenticatedUser == null)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors("Provided username/password combination is invalid");
                });
            }

            var token = _tokenService.GenerateJwtTokenFor(authenticatedUser);
            return _builder.CreateSuccessResponse(token, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Token generated successfully");
            });
        }
    }
}