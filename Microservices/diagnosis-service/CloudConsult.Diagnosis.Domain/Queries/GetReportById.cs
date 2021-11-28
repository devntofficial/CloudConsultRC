using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Diagnosis.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Diagnosis.Domain.Queries
{
    public class GetReportById : IQuery<ReportResponse?>
    {
        public GetReportById(string reportId)
        {
            ReportId = reportId;
        }

        public string ReportId { get; set; } = "";
    }

    public class GetReportByIdValidator : ApiValidator<GetReportById>
    {
        public GetReportByIdValidator()
        {
            RuleFor(x => x.ReportId).Must(BeValidMongoDbId).WithMessage("Invalid 'ReportId' format");
        }
    }
}
