using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using FluentValidation;

namespace CloudConsult.Consultation.Domain.Commands
{
    public class CancelConsultation : ICommand
    {
        public string ConsultationId { get; set; }
    }

    public class CancelConsultationValidator : ApiValidator<CancelConsultation>
    {
        public CancelConsultationValidator()
        {
            RuleFor(x => x.ConsultationId).NotEmpty();
            RuleFor(x => x.ConsultationId).Must(BeValidGuid).WithMessage("ConsultationId has invalid format");
        }
    }
}
