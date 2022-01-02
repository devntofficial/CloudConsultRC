using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Diagnosis.Domain.Commands;
using CloudConsult.Diagnosis.Domain.Entities;
using CloudConsult.Diagnosis.Domain.Responses;
using CloudConsult.Diagnosis.Domain.Services;
using CloudConsult.Diagnosis.Infrastructure.Clients;
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
        private readonly ConsultationApiClient consultationApiClient;

        public UploadReportHandler(IApiResponseBuilder<UploadReportResponse?> builder, IReportService reportService,
            IValidator<UploadReport> validator, IMapper mapper, ConsultationApiClient consultationApiClient)
        {
            this.builder = builder;
            this.reportService = reportService;
            this.validator = validator;
            this.mapper = mapper;
            this.consultationApiClient = consultationApiClient;
        }

        public async Task<IApiResponse<UploadReportResponse?>> Handle(UploadReport request, CancellationToken cancellationToken)
        {
            var validation = await validator.ValidateAsync(request, cancellationToken);

            if (validation.Errors.Any())
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                });
            }

            var consultationApiResponse = await consultationApiClient.GetConsultationAsync(request.ConsultationId, cancellationToken);
            if(consultationApiResponse.IsSuccess == false)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrors(consultationApiResponse.Errors);
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                });
            }

            var consultation = consultationApiResponse.Payload;
            var mappedReport = mapper.Map<DiagnosisReport>(request);

            mappedReport.DoctorProfileId = consultation.DoctorProfileId;
            mappedReport.DoctorName = consultation.DoctorName;
            mappedReport.DoctorEmailId = consultation.DoctorEmailId;
            mappedReport.DoctorMobileNo = consultation.DoctorMobileNo;

            mappedReport.MemberProfileId = consultation.MemberProfileId;
            mappedReport.MemberName = consultation.MemberName;
            mappedReport.MemberEmailId = consultation.MemberEmailId;
            mappedReport.MemberMobileNo = consultation.MemberMobileNo;

            var savedReport = await reportService.Upload(mappedReport, cancellationToken);

            var response = mapper.Map<UploadReportResponse>(savedReport);
            return builder.CreateSuccessResponse(response, x =>
            {
                x.WithMessages("Diagnosis report uploaded successfully");
                x.WithSuccessCode(StatusCodes.Status201Created);
            });

            
        }
    }
}
