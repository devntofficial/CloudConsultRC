using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using FluentValidation;

namespace CloudConsult.Consultation.Domain.Commands;

public record RequestConsultation : ICommand<String>
{
    public string DoctorProfileId { get; set; }
    public string DoctorName { get; set; }
    public string DoctorEmailId { get; set; }
    public string DoctorMobileNo { get; set; }
    public string MemberProfileId { get; set; }
    public string MemberName { get; set; }
    public string MemberEmailId { get; set; }
    public string MemberMobileNo { get; set; }
    public string TimeSlotId { get; set; }
    public string Description { get; set; }
}

public class RequestConsultationValidator : ApiValidator<RequestConsultation>
{
    public RequestConsultationValidator()
    {
        RuleFor(x => x.DoctorProfileId).NotEmpty();
        RuleFor(x => x.DoctorName).NotEmpty();
        RuleFor(x => x.MemberProfileId).NotEmpty();
        RuleFor(x => x.MemberName).NotEmpty();
    }
}