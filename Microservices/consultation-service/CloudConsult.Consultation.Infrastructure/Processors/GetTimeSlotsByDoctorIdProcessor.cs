using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IApiResponseBuilder<TimeSlotResponse> _builder;
        private readonly ITimeSlotService _service;
        private readonly IMapper _mapper;

        public GetTimeSlotsByDoctorIdProcessor(IApiResponseBuilder<TimeSlotResponse> builder,
            ITimeSlotService service, IMapper mapper)
        {
            _builder = builder;
            _service = service;
            _mapper = mapper;
        }
        
        public async Task<IApiResponse<TimeSlotResponse>> Handle(GetTimeSlotsByDoctorId request, CancellationToken cancellationToken)
        {
            var output = await _service.GetDoctorAvailability(request.DoctorId, cancellationToken)
                ;

            if (output.Any())
            {
                var response = _mapper.Map<TimeSlotResponse>(output);

                return _builder.CreateSuccessResponse(response, x =>
                {
                    x.WithSuccessCode(StatusCodes.Status200OK);
                    x.WithMessages("Availability data fetched successfully");
                });
            }

            return _builder.CreateErrorResponse(null, x =>
            {
                x.WithErrorCode(StatusCodes.Status404NotFound);
                x.WithErrors("No availability records found for given doctor id");
            });
        }
    }
}