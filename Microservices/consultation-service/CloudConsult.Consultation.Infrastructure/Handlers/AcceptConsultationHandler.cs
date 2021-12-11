using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Consultation.Infrastructure.Handlers
{
    public class AcceptConsultationHandler : ICommandHandler<AcceptConsultation>
    {
        private readonly IApiResponseBuilder builder;
        private readonly IConsultationService consultationService;
        private readonly IValidator<AcceptConsultation> validator;

        public AcceptConsultationHandler(IApiResponseBuilder builder, IConsultationService consultationService,
            IValidator<AcceptConsultation> validator)
        {
            this.builder = builder;
            this.consultationService = consultationService;
            this.validator = validator;
        }

        public async Task<IApiResponse> Handle(AcceptConsultation request, CancellationToken cancellationToken)
        {
            var dataValidation = await validator.ValidateAsync(request, cancellationToken);
            if (dataValidation.Errors.Any())
            {
                return builder.CreateErrorResponse(x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(dataValidation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var response = await consultationService.Accept(request.ConsultationId, cancellationToken);

            if (!response.IsSuccess)
            {
                return builder.CreateErrorResponse(x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(response.Status);
                });
            }

            return builder.CreateSuccessResponse(x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages(response.Status);
            });
        }
    }
}
