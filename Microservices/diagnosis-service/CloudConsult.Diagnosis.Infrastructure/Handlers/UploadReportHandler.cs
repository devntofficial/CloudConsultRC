using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Diagnosis.Domain.Commands;
using CloudConsult.Diagnosis.Domain.Entities;
using CloudConsult.Diagnosis.Domain.Responses;
using CloudConsult.Diagnosis.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Diagnosis.Infrastructure.Handlers
{
    public class UploadReportHandler : ICommandHandler<UploadReport, UploadReportResponse?>
    {
        private readonly IApiResponseBuilder<UploadReportResponse?> builder;
        private readonly IReportService reportService;
        private readonly IValidator<UploadReport> validator;
        private readonly IMapper mapper;

        public UploadReportHandler(IApiResponseBuilder<UploadReportResponse?> builder, IReportService reportService,
            IValidator<UploadReport> validator, IMapper mapper)
        {
            this.builder = builder;
            this.reportService = reportService;
            this.validator = validator;
            this.mapper = mapper;
        }

        public async Task<IApiResponse<UploadReportResponse?>> Handle(UploadReport request, CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);

            if (validation.IsValid)
            {
                var mappedReport = mapper.Map<DiagnosisReport>(request);
                var savedReport = await reportService.Upload(mappedReport, cancellationToken);

                var response = mapper.Map<UploadReportResponse>(savedReport);
                return builder.CreateSuccessResponse(response, x =>
                {
                    x.WithMessages("Diagnosis report uploaded successfully");
                    x.WithSuccessCode(StatusCodes.Status201Created);
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
