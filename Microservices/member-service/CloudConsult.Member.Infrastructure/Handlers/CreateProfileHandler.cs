using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Member.Domain.Commands;
using CloudConsult.Member.Domain.Entities;
using CloudConsult.Member.Domain.Responses;
using CloudConsult.Member.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Handlers
{
    public class CreateProfileHandler : ICommandHandler<CreateProfile, ProfileResponse>
    {
        private readonly IApiResponseBuilder<ProfileResponse> _builder;
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateProfile> _validator;

        public CreateProfileHandler(IApiResponseBuilder<ProfileResponse> builder,
            IProfileService profileService, IMapper mapper, IValidator<CreateProfile> validator)
        {
            _builder = builder;
            _profileService = profileService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IApiResponse<ProfileResponse>> Handle(CreateProfile request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var mappedProfile = _mapper.Map<MemberProfile>(request);

            var createdProfile = await _profileService.Create(mappedProfile, cancellationToken);

            var response = _mapper.Map<ProfileResponse>(createdProfile);

            return _builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status201Created);
                x.WithMessages("Profile created successfully");
            });
        }
    }
}