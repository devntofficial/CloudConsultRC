using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Consultation.Infrastructure.Handlers
{
    public class BookConsultationHandler : ICommandHandler<BookConsultationCommand, String>
    {
        private readonly IApiResponseBuilder<String> _builder;
        private readonly IConsultationService _consultationService;
        private readonly IMapper _mapper;
        private readonly IValidator<BookConsultationCommand> _validator;

        public BookConsultationHandler(
            IApiResponseBuilder<String> builder,
            IConsultationService consultationService,
            IMapper mapper,
            IValidator<BookConsultationCommand> validator)
        {
            _builder = builder;
            _consultationService = consultationService;
            _mapper = mapper;
            _validator = validator;
        }
        
        public async Task<IApiResponse<String>> Handle(BookConsultationCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken);

            if (!validation.IsValid)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var mappedBooking = _mapper.Map<ConsultationBookingEntity>(request);

            var response = await _consultationService.BookConsultation(mappedBooking, cancellationToken);

            if (Guid.TryParse(response, out var id))
            {
                return _builder.CreateSuccessResponse(response, x =>
                {
                    x.WithSuccessCode(StatusCodes.Status201Created);
                    x.WithMessages("Consultation booked successfully");
                });
            }

            return _builder.CreateErrorResponse(null, x =>
            {
                x.WithErrorCode(StatusCodes.Status400BadRequest);
                x.WithErrors(response);
            });

        }
    }
}