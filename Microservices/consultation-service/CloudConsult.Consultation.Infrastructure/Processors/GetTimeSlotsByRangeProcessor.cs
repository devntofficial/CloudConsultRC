using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Queries;
using CloudConsult.Consultation.Domain.Responses;
using CloudConsult.Consultation.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Consultation.Infrastructure.Processors
{
    public class GetTimeSlotsByRangeProcessor : IQueryProcessor<GetTimeSlotsByRange, TimeSlotRangeResponse>
    {
        private readonly IApiResponseBuilder<TimeSlotRangeResponse> builder;
        private readonly ITimeSlotService service;
        private readonly IMapper mapper;

        public GetTimeSlotsByRangeProcessor(IApiResponseBuilder<TimeSlotRangeResponse> builder,
            ITimeSlotService service, IMapper mapper)
        {
            this.builder = builder;
            this.service = service;
            this.mapper = mapper;
        }

        public async Task<IApiResponse<TimeSlotRangeResponse>> Handle(GetTimeSlotsByRange request, CancellationToken cancellationToken)
        {
            var output = await service.GetDoctorTimeSlotsRange(request.ProfileId, request.StartDateTime, request.EndDateTime, cancellationToken);
            var response = new TimeSlotRangeResponse { ProfileId = request.ProfileId, TimeSlots = new() };
            if (output.Any())
            {
                response.TimeSlots = mapper.Map<List<TimeSlot>>(output);
                return builder.CreateSuccessResponse(response, x =>
                {
                    x.WithSuccessCode(StatusCodes.Status200OK);
                    x.WithMessages("Time slots fetched successfully");
                });
            }

            return builder.CreateErrorResponse(response, x =>
            {
                x.WithErrorCode(StatusCodes.Status404NotFound);
                x.WithErrors("No time slots found for given doctor id");
            });
        }
    }
}
