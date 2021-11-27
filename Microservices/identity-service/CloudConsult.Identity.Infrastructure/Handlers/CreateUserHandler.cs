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
        private readonly IApiResponseBuilder<CreateUserResponse> _builder;
        private readonly IValidator<CreateUser> _validator;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public CreateUserHandler(IApiResponseBuilder<CreateUserResponse> builder,
            IValidator<CreateUser> validator, IMapper mapper,
            IIdentityService identityService)
        {
            _builder = builder;
            _validator = validator;
            _mapper = mapper;
            _identityService = identityService;
        }
        public async Task<IApiResponse<CreateUserResponse>> Handle(CreateUser request, CancellationToken cancellationToken)
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

            var createdUser = await _identityService.Create(request, cancellationToken);
            if (createdUser == null)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors("Could not create user. Please contact customer support.");
                });
            }

            var response = _mapper.Map<CreateUserResponse>(createdUser);
            return _builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status201Created);
                x.WithMessages("User created successfully.");
            });
        }
    }
}