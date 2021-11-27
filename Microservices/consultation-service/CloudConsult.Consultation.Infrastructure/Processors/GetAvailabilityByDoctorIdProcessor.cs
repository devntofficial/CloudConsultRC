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
    public class GetAvailabilityByDoctorIdProcessor : IQueryProcessor<GetAvailabilityByDoctorId, DoctorAvailabilityResponse>
    {
        private readonly IApiResponseBuilder<DoctorAvailabilityResponse> _builder;
        private readonly IAvailabilityService _service;
        private readonly IMapper _mapper;

        public GetAvailabilityByDoctorIdProcessor(IApiResponseBuilder<DoctorAvailabilityResponse> builder,
            IAvailabilityService service, IMapper mapper)
        {
            _builder = builder;
            _service = service;
            _mapper = mapper;
        }
        
        public async Task<IApiResponse<DoctorAvailabilityResponse>> Handle(GetAvailabilityByDoctorId request, CancellationToken cancellationToken)
        {
            var output = await _service.GetDoctorAvailability(request.DoctorId, cancellationToken)
                ;

            if (output.Any())
            {
                var response = _mapper.Map<DoctorAvailabilityResponse>(output);

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