using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Identity.Domain.Commands;
using CloudConsult.Identity.Domain.Responses;
using CloudConsult.Identity.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Identity.Infrastructure.Handlers
{
    public class CreateUserHandler : ICommandHandler<CreateUser, CreateUserResponse>
    {
        private readonly IApiResponseBuilder<CreateUserResponse> builder;
        private readonly IValidator<CreateUser> validator;
        private readonly IMapper mapper;
        private readonly IIdentityService identityService;
        private readonly ITokenService tokenService;

        public CreateUserHandler(IApiResponseBuilder<CreateUserResponse> builder,
            IValidator<CreateUser> validator, IMapper mapper,
            IIdentityService identityService, ITokenService tokenService)
        {
            this.builder = builder;
            this.validator = validator;
            this.mapper = mapper;
            this.identityService = identityService;
            this.tokenService = tokenService;
        }
        public async Task<IApiResponse<CreateUserResponse>> Handle(CreateUser request, CancellationToken cancellationToken)
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

            var createdUser = await identityService.Create(request, cancellationToken);
            if (!createdUser.created)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(createdUser.message);
                });
            }

            var token = tokenService.GenerateJwtTokenFor(createdUser.user);
            var response = mapper.Map<CreateUserResponse>(createdUser.user);
            response.AccessToken = token.AccessToken;

            return builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status201Created);
                x.WithMessages("User created successfully.");
            });
        }
    }
}