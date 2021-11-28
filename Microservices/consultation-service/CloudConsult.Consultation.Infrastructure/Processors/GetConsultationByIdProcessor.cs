using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Queries;
using CloudConsult.Consultation.Domain.Responses;
using CloudConsult.Consultation.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Consultation.Infrastructure.Processors
{
    public class GetConsultationByIdProcessor : IQueryProcessor<GetConsultationById, GetConsultationByIdResponse>
    {
        private readonly IApiResponseBuilder<GetConsultationByIdResponse> _builder;
        private readonly IMapper _mapper;
        private readonly IConsultationService _consultationService;

        public GetConsultationByIdProcessor(IApiResponseBuilder<GetConsultationByIdResponse> builder,
            IMapper mapper, IConsultationService consultationService)
        {
            _builder = builder;
            _mapper = mapper;
            _consultationService = consultationService;
        }

        public async Task<IApiResponse<GetConsultationByIdResponse>> Handle(GetConsultationById request,
            CancellationToken cancellationToken)
        {
            var output = await _consultationService.GetById(request.ConsultationId, cancellationToken);
            if (output == null)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors("No data found for given id");
                });
            }

            //call doctor and patient grpc servers to get name or store them when booking consultation

            return _builder.CreateSuccessResponse(output, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Consultation data fetched successfully");
            });
        }
    }
}