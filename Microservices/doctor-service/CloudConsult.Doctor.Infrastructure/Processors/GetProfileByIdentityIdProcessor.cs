﻿using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Queries;
using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Doctor.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Processors
{
    public class GetProfileByIdentityIdProcessor : IQueryProcessor<GetProfileByIdentityId, ProfileResponse>
    {
        private readonly IApiResponseBuilder<ProfileResponse> _builder;
        private readonly IProfileService _profileService;
        private readonly IMapper _mapper;

        public GetProfileByIdentityIdProcessor(IApiResponseBuilder<ProfileResponse> builder, IProfileService profileService,
            IMapper mapper)
        {
            _builder = builder;
            _profileService = profileService;
            _mapper = mapper;
        }

        public async Task<IApiResponse<ProfileResponse>> Handle(GetProfileByIdentityId request,CancellationToken cancellationToken)
        {
            var output = await _profileService.GetByIdentityId(request.IdentityId, cancellationToken);

            if (output == null)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors($"No profile found with identity id: {request.IdentityId}");
                });
            }

            var response = _mapper.Map<ProfileResponse>(output);
            return _builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Doctor profile for given identity id is fetched successfully");
            });
        }
    }
}