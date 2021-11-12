using CloudConsult.Common.CQRS;
using CloudConsult.Patient.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Patient.Domain.Commands
{
    public record CreatePatientCommand : ICommand<CreatePatientResponse>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
    }

    public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Gender).NotEmpty();
            RuleFor(x => x.EmailId).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.AadhaarNo).NotEmpty().Must(BeInValidFormat);
        }

        private bool BeInValidFormat(string aadhaarNo)
        {
            //check numeric
            //check (-) are in place
            //govt site check
            return true;
        }
    }
}
