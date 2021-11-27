using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Doctor.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Handlers
{
    public class UpdateProfileHandler : ICommandHandler<UpdateProfile, ProfileResponse>
    {

        private readonly IApiResponseBuilder<ProfileResponse> _builder;
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateProfile> _validator;


        public UpdateProfileHandler(IApiResponseBuilder<ProfileResponse> builder,
            IProfileService profileService, IMapper mapper, IValidator<UpdateProfile> validator)
        {
            _builder = builder;
            _profileService = profileService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IApiResponse<ProfileResponse>> Handle(UpdateProfile request, CancellationToken cancellationToken)
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

            var mappedProfile = _mapper.Map<UpdateProfile, DoctorProfile>(request);

            var updatedProfile = await _profileService.Update(mappedProfile, cancellationToken);

            var response = _mapper.Map<ProfileResponse>(updatedProfile);
            if (response == null)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors("Profile with given Id not found!");
                });
            }

            return _builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Profile updated successfully");
            });
        }
    }
}
