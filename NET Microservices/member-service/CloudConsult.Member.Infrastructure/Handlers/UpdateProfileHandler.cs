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
    public class UpdateProfileHandler : ICommandHandler<UpdateProfile, ProfileResponse>
    {

        private readonly IApiResponseBuilder<ProfileResponse> builder;
        private readonly IProfileService profileService;
        private readonly IMapper mapper;
        private readonly IValidator<UpdateProfile> validator;


        public UpdateProfileHandler(IApiResponseBuilder<ProfileResponse> builder,
            IProfileService profileService, IMapper mapper, IValidator<UpdateProfile> validator)
        {
            this.builder = builder;
            this.profileService = profileService;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<IApiResponse<ProfileResponse>> Handle(UpdateProfile request, CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);
            if (!validation.IsValid)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var mappedProfile = mapper.Map<UpdateProfile, MemberProfile>(request);

            var updatedProfile = await profileService.Update(mappedProfile, cancellationToken);

            var response = mapper.Map<ProfileResponse>(updatedProfile);
            if (response == null)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors("Profile with given Id not found!");
                });
            }

            return builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Profile updated successfully");
            });
        }
    }
}
