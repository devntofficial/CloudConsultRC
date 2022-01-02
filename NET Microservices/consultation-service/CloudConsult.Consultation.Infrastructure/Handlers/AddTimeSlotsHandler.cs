using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Consultation.Infrastructure.Handlers
{
    public class AddTimeSlotsHandler : ICommandHandler<AddTimeSlots, object>
    {
        private readonly IApiResponseBuilder<object> builder;
        private readonly IMapper mapper;
        private readonly ITimeSlotService timeSlotService;

        public AddTimeSlotsHandler(IApiResponseBuilder<object> builder, IMapper mapper, ITimeSlotService timeSlotService)
        {
            this.builder = builder;
            this.mapper = mapper;
            this.timeSlotService = timeSlotService;
        }
        
        public async Task<IApiResponse<object>> Handle(AddTimeSlots request, CancellationToken cancellationToken)
        {
            var availabilities = mapper.Map<List<DoctorTimeSlot>>(request);
            await timeSlotService.AddDoctorTimeSlots(availabilities, cancellationToken);

            return builder.CreateSuccessResponse(null, x =>
            {
                x.WithSuccessCode(StatusCodes.Status202Accepted);
                x.WithMessages("Doctor availability saved successfully");
            });
        }
    }
}