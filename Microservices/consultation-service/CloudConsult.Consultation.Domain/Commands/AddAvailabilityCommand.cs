using System.Collections.Generic;
using CloudConsult.Common.CQRS;
using FluentValidation;

namespace CloudConsult.Consultation.Domain.Commands
{
    public record AddAvailabilityCommand : ICommand<object>
    {
        public string DoctorId { get; set; }
        public Dictionary<string, List<string>> AvailabilityMap { get; set; }
    }

    public class AddAvailabilityCommandValidator : AbstractValidator<AddAvailabilityCommand>
    {
        public AddAvailabilityCommandValidator()
        {
            RuleFor(x => x.DoctorId).NotEmpty();
            RuleFor(x => x.AvailabilityMap).Must(HaveSomeValues);
        }

        private bool HaveSomeValues(Dictionary<string, List<string>> map)
        {
            return map != null && map.Count != 0;
        }
    }
}