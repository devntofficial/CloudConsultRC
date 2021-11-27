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
    public class AddAvailabilityHandler : ICommandHandler<AddAvailability, object>
    {
        private readonly IApiResponseBuilder<object> _builder;
        private readonly IMapper _mapper;
        private readonly IAvailabilityService _availabilityService;

        public AddAvailabilityHandler(IApiResponseBuilder<object> builder, IMapper mapper, IAvailabilityService availabilityService)
        {
            _builder = builder;
            _mapper = mapper;
            _availabilityService = availabilityService;
        }
        
        public async Task<IApiResponse<object>> Handle(AddAvailability request, CancellationToken cancellationToken)
        {
            var availabilities = _mapper.Map<List<DoctorAvailability>>(request);
            await _availabilityService.AddDoctorAvailabilities(availabilities, cancellationToken);

            return _builder.CreateSuccessResponse(null, x =>
            {
                x.WithSuccessCode(StatusCodes.Status202Accepted);
                x.WithMessages("Doctor availability saved successfully");
            });
        }
    }
}