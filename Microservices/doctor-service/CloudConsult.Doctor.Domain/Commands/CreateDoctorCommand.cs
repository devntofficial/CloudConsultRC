using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Common.CQRS;
using FluentValidation;

namespace CloudConsult.Doctor.Domain.Commands
{
    public record CreateDoctorCommand : ICommand<CreateDoctorResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
    }

    public class CreateDoctorCommandValidator : AbstractValidator<CreateDoctorCommand>
    {
        public CreateDoctorCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
        }
    }
}