using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Queries;
using CloudConsult.Consultation.Domain.Responses;
using CloudConsult.Consultation.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Consultation.Infrastructure.Processors
{
    public class GetConsultationsByDoctorIdProcessor : IQueryProcessor<GetConsultationsByDoctorId, ConsultationResponse>
    {
        private readonly IApiResponseBuilder<ConsultationResponse> builder;
        private readonly IConsultationService consultationService;
        private readonly IMapper mapper;

        public GetConsultationsByDoctorIdProcessor(IApiResponseBuilder<ConsultationResponse> builder,
            IConsultationService consultationService, IMapper mapper)
        {
            this.builder = builder;
            this.consultationService = consultationService;
            this.mapper = mapper;
        }

        public async Task<IApiResponse<ConsultationResponse>> Handle(GetConsultationsByDoctorId request, CancellationToken cancellationToken)
        {
            var consultations = await consultationService.GetByDoctorId(request.DoctorId, cancellationToken);
            var response = mapper.Map<ConsultationResponse>(consultations);

            if(response.Consultations.Count == 0)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrors("No consultations found for given doctor id");
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                });
            }

            return builder.CreateSuccessResponse(response, x =>
            {
                x.WithMessages("Consultations for given doctor id fetched successfully");
                x.WithSuccessCode(StatusCodes.Status200OK);
            });
        }
    }
}
