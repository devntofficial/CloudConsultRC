using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Queries;
using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Doctor.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Processors
{
    public class GetAllProfilesProcessor : IPaginatedQueryProcessor<GetAllProfiles, List<ProfileResponse>>
    {
        private readonly IApiResponseBuilder<List<ProfileResponse>> builder;
        private readonly IProfileService profileService;
        private readonly IMapper mapper;

        public GetAllProfilesProcessor(IApiResponseBuilder<List<ProfileResponse>> builder, IProfileService profileService,
            IMapper mapper)
        {
            this.builder = builder;
            this.profileService = profileService;
            this.mapper = mapper;
        }

        public async Task<IApiResponse<List<ProfileResponse>>> Handle(GetAllProfiles request, CancellationToken cancellationToken)
        {
            var output = await profileService.GetAllPaginated(request.PageNo, request.PageSize, cancellationToken);

            if (output == null)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors($"No profiles found");
                });
            }

            var response = mapper.Map<List<ProfileResponse>>(output);
            return builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Profiles fetched successfully");
            });
        }
    }
}
