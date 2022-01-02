using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Queries;
using CloudConsult.Consultation.Domain.Responses;
using CloudConsult.Consultation.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Consultation.Infrastructure.Processors
{
    public class GetTimeSlotsByDoctorIdProcessor : IQueryProcessor<GetTimeSlotsByDoctorId, TimeSlotResponse>
    {
        private readonly IApiResponseBuilder<TimeSlotResponse> builder;
        private readonly ITimeSlotService service;
        private readonly IMapper mapper;

        public GetTimeSlotsByDoctorIdProcessor(IApiResponseBuilder<TimeSlotResponse> builder,
            ITimeSlotService service, IMapper mapper)
        {
            this.builder = builder;
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IApiResponse<TimeSlotResponse>> Handle(GetTimeSlotsByDoctorId request, CancellationToken cancellationToken)
        {
            var output = await service.GetDoctorTimeSlots(request.DoctorId, cancellationToken);

            if (output.Any())
            {
                var response = mapper.Map<TimeSlotResponse>(output);

                return builder.CreateSuccessResponse(response, x =>
                {
                    x.WithSuccessCode(StatusCodes.Status200OK);
                    x.WithMessages("Time slots fetched successfully");
                });
            }

            return builder.CreateErrorResponse(null, x =>
            {
                x.WithErrorCode(StatusCodes.Status404NotFound);
                x.WithErrors("No time slots found for given doctor id");
            });
        }
    }
}