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
    public class RequestConsultationHandler : ICommandHandler<RequestConsultation, String>
    {
        private readonly IApiResponseBuilder<string> builder;
        private readonly IConsultationService consultationService;
        private readonly IMapper mapper;
        private readonly IValidator<RequestConsultation> validator;

        public RequestConsultationHandler(IApiResponseBuilder<string> builder, IConsultationService consultationService,
            IMapper mapper, IValidator<RequestConsultation> validator)
        {
            this.builder = builder;
            this.consultationService = consultationService;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<IApiResponse<string>> Handle(RequestConsultation request, CancellationToken cancellationToken)
        {
            var dataValidation = await validator.ValidateAsync(request, cancellationToken);
            if (dataValidation.Errors.Any())
            {
                return builder.CreateErrorResponse(string.Empty, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(dataValidation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var consultation = mapper.Map<ConsultationRequest>(request);
            var response = await consultationService.Request(consultation, cancellationToken);

            if (!response.IsSuccess)
            {
                return builder.CreateErrorResponse(string.Empty, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(response.Status);
                });
            }

            return builder.CreateSuccessResponse(response.ConsultationId, x =>
            {
                x.WithSuccessCode(StatusCodes.Status201Created);
                x.WithMessages(response.Status);
            });
        }
    }
}