using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Diagnosis.Domain.Responses;
using FluentValidation;
using System.Text.Json.Serialization;

namespace CloudConsult.Diagnosis.Domain.Commands
{
    public class UploadReport : ICommand<UploadReportResponse?>
    {
        [JsonIgnore]
        public string ConsultationId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public List<MedicinePrescription> MedicinePrescriptions { get; set; } = new();
    }

    public class MedicinePrescription
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
    }

    public class UploadReportValidator : ApiValidator<UploadReport>
    {
        public UploadReportValidator()
        {
            RuleFor(x => x.ConsultationId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Summary).NotEmpty();
        }
    }
}
