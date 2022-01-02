﻿using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Member.Domain.Queries;
using CloudConsult.Member.Domain.Responses;
using CloudConsult.Member.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Processors
{
    public class GetProfileByIdProcessor : IQueryProcessor<GetProfileById, ProfileResponse>
    {
        private readonly IApiResponseBuilder<ProfileResponse> builder;
        private readonly IProfileService profileService;
        private readonly IMapper mapper;

        public GetProfileByIdProcessor(IApiResponseBuilder<ProfileResponse> builder, IProfileService profileService,
            IMapper mapper)
        {
            this.builder = builder;
            this.profileService = profileService;
            this.mapper = mapper;
        }

        public async Task<IApiResponse<ProfileResponse>> Handle(GetProfileById request, CancellationToken cancellationToken)
        {
            var output = await profileService.GetById(request.ProfileId, cancellationToken);

            if (output == null)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors($"No profile found with id: {request.ProfileId}");
                });
            }

            var response = mapper.Map<ProfileResponse>(output);
            return builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Member profile for given profile id is fetched successfully");
            });
        }
    }
}