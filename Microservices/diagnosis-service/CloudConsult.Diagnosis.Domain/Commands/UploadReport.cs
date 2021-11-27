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
        public string ConsultationId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Summary { get; set; } = "";
        public List<MedicinePrescription> MedicinePrescriptions { get; set; } = new();
    }

    public class MedicinePrescription
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "";
        public string Dosage { get; set; } = "";
        public string Duration { get; set; } = "";
        public string Frequency { get; set; } = "";
        public string Remarks { get; set; } = "";
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
