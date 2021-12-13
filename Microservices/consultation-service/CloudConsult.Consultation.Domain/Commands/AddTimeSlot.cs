﻿using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using FluentValidation;

namespace CloudConsult.Consultation.Domain.Commands;

public class AddTimeSlot : ICommand<object>
{
    public string DoctorProfileId { get; set; }
    public Dictionary<string, List<string>> AvailabilityMap { get; set; }
}

public class AddAvailabilityValidator : ApiValidator<AddTimeSlot>
{
    public AddAvailabilityValidator()
    {
        RuleFor(x => x.DoctorProfileId).NotEmpty();
        RuleFor(x => x.AvailabilityMap).Must(HaveSomeValues);
    }
}