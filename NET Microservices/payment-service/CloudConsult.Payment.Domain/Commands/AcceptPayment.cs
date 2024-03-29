﻿using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using CloudConsult.Payment.Domain.Responses;
using FluentValidation;

namespace CloudConsult.Payment.Domain.Commands
{
    public class AcceptPayment : ICommand<PaymentResponse?>
    {
        public string ConsultationId { get; set; } = string.Empty;
    }

    public class AcceptPaymentValidator : ApiValidator<AcceptPayment>
    {
        public AcceptPaymentValidator()
        {
            RuleFor(x => x.ConsultationId).NotEmpty();
            RuleFor(x => x.ConsultationId).Must(BeValidGuid).WithMessage("Invalid ConsultationId format");
        }
    }
}
