using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Diagnosis.Domain.Queries;
using CloudConsult.Diagnosis.Domain.Responses;
using CloudConsult.Diagnosis.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Diagnosis.Infrastructure.Processors
{
    public class GetReportByConsultationIdProcessor : IQueryProcessor<GetReportByConsultationId, ReportResponse?>
    {
        private readonly IApiResponseBuilder<ReportResponse?> builder;
        private readonly IReportService reportService;
        private readonly IValidator<GetReportByConsultationId> validator;
        private readonly IMapper mapper;

        public GetReportByConsultationIdProcessor(IApiResponseBuilder<ReportResponse?> builder, IReportService reportService,
            IValidator<GetReportByConsultationId> validator, IMapper mapper)
        {
            this.builder = builder;
            this.reportService = reportService;
            this.validator = validator;
            this.mapper = mapper;
        }

        public async Task<IApiResponse<ReportResponse?>> Handle(GetReportByConsultationId request, CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);

            if (validation.IsValid)
            {
                var report = await reportService.GetByConsultationId(request.ConsultationId, cancellationToken);

                if (report is null)
                {
                    return builder.CreateErrorResponse(null, x =>
                    {
                        x.WithErrors("No diagnosis report found with given consultation id");
                        x.WithErrorCode(StatusCodes.Status404NotFound);
                    });
                }

                var response = mapper.Map<ReportResponse>(report);
                return builder.CreateSuccessResponse(response, x =>
                {
                    x.WithMessages("Diagnosis report fetched successfully");
                    x.WithSuccessCode(StatusCodes.Status200OK);
                });

            }

            return builder.CreateErrorResponse(null, x =>
            {
                x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                x.WithErrorCode(StatusCodes.Status400BadRequest);
            });
        }
    }
}
