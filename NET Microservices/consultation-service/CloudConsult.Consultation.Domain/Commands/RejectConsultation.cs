using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using FluentValidation;

namespace CloudConsult.Consultation.Domain.Commands
{
    public class RejectConsultation : ICommand
    {
        public string ConsultationId { get; set; }
    }

    public class RejectConsultationValidator : ApiValidator<RejectConsultation>
    {
        public RejectConsultationValidator()
        {
            RuleFor(x => x.ConsultationId).NotEmpty();
            RuleFor(x => x.ConsultationId).Must(BeValidGuid).WithMessage("ConsultationId has invalid format");
        }
    }
}
