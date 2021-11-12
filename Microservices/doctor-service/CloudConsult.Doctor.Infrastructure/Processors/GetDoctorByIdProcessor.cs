using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CloudConsult.Doctor.Domain.Queries;
using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Doctor.Domain.Services;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Processors
{
    public class GetDoctorByIdProcessor : IQueryProcessor<GetDoctorByIdQuery, CreateDoctorResponse>
    {
        private readonly IApiResponseBuilder<CreateDoctorResponse> _builder;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;

        public GetDoctorByIdProcessor(IApiResponseBuilder<CreateDoctorResponse> builder, IDoctorService doctorService,
            IMapper mapper)
        {
            _builder = builder;
            _doctorService = doctorService;
            _mapper = mapper;
        }

        public async Task<IApiResponse<CreateDoctorResponse>> Handle(GetDoctorByIdQuery request,
            CancellationToken cancellationToken)
        {
            var output = await _doctorService.GetById(request.DoctorId, cancellationToken).ConfigureAwait(false);

            if (output == null)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors($"No doctor found with id: {request.DoctorId}");
                });
            }

            var response = _mapper.Map<CreateDoctorResponse>(output);
            return _builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Doctor details for given id is fetched successfully");
            });
        }
    }
}