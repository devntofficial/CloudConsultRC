using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Diagnosis.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Diagnosis.Domain.Queries
{
    public class GetReportByConsultationId : IQuery<ReportResponse?>
    {
        public GetReportByConsultationId(string consultationId)
        {
            ConsultationId = consultationId;
        }

        public string ConsultationId { get; set; } = string.Empty;
    }

    public class GetReportByConsultationIdValidator : ApiValidator<GetReportByConsultationId>
    {
        public GetReportByConsultationIdValidator()
        {
            RuleFor(x => x.ConsultationId).Must(BeValidGuid).WithMessage("Invalid 'ConsultationId' format");
        }
    }
}
