using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Responses;
using CloudConsult.Consultation.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Consultation.Infrastructure.Handlers
{
    public class AddSingleTimeSlotHandler : ICommandHandler<AddSingleTimeSlot, TimeSlot>
    {
        private readonly IApiResponseBuilder<TimeSlot> builder;
        private readonly ITimeSlotService timeSlotService;

        public AddSingleTimeSlotHandler(IApiResponseBuilder<TimeSlot> builder, ITimeSlotService timeSlotService)
        {
            this.builder = builder;
            this.timeSlotService = timeSlotService;
        }

        public async Task<IApiResponse<TimeSlot>> Handle(AddSingleTimeSlot request, CancellationToken cancellationToken)
        {
            var output = await timeSlotService.AddSingleTimeSlot(request, cancellationToken);
            if(output.isSuccess)
            {
                return builder.CreateSuccessResponse(output.response, x =>
                {
                    x.WithSuccessCode(StatusCodes.Status200OK);
                    x.WithMessages(output.message);
                });
            }
            return builder.CreateErrorResponse(output.response, x =>
            {
                x.WithErrorCode(StatusCodes.Status400BadRequest);
                x.WithErrors(output.message);
            });
        }
    }
}
