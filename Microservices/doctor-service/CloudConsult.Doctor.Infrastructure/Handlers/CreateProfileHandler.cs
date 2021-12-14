using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Doctor.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Handlers;

public class CreateProfileHandler : ICommandHandler<CreateProfile, ProfileResponse>
{
    private readonly IApiResponseBuilder<ProfileResponse> builder;
    private readonly IProfileService profileService;
    private readonly IMapper mapper;
    private readonly IValidator<CreateProfile> validator;

    public CreateProfileHandler(IApiResponseBuilder<ProfileResponse> builder,
        IProfileService profileService, IMapper mapper, IValidator<CreateProfile> validator)
    {
        this.builder = builder;
        this.profileService = profileService;
        this.mapper = mapper;
        this.validator = validator;
    }

    public async Task<IApiResponse<ProfileResponse>> Handle(CreateProfile request, CancellationToken cancellationToken)
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

        var createdProfile = await profileService.Create(mapper.Map<DoctorProfile>(request), cancellationToken);
        if (createdProfile is null)
        {
            return builder.CreateErrorResponse(null, x =>
            {
                x.WithErrorCode(StatusCodes.Status400BadRequest);
                x.WithErrors("A profile already exists for this identity");
            });
        }

        return builder.CreateSuccessResponse(mapper.Map<ProfileResponse>(createdProfile), x =>
        {
            x.WithSuccessCode(StatusCodes.Status201Created);
            x.WithMessages("Profile created successfully");
        });
    }
}