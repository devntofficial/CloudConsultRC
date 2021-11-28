using System.Collections.Generic;
using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using FluentValidation;

namespace CloudConsult.Consultation.Domain.Commands
{
    public record AddAvailability : ICommand<object>
    {
        public string DoctorId { get; set; }
        public Dictionary<string, List<string>> AvailabilityMap { get; set; }
    }

    public class AddAvailabilityValidator : ApiValidator<AddAvailability>
    {
        public AddAvailabilityValidator()
        {
            RuleFor(x => x.DoctorId).NotEmpty();
            RuleFor(x => x.AvailabilityMap).Must(HaveSomeValues);
        }
    }
}