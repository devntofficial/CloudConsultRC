using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Patient.Domain.Commands;
using CloudConsult.Patient.Domain.Entities;
using CloudConsult.Patient.Domain.Responses;
using CloudConsult.Patient.Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Patient.Infrastructure.Handlers
{
    public class CreatePatientHandler : ICommandHandler<CreatePatientCommand, CreatePatientResponse>
    {
        private readonly IApiResponseBuilder<CreatePatientResponse> _builder;
        private readonly IValidator<CreatePatientCommand> _validator;
        private readonly IMapper _mapper;
        private readonly IPatientService _patientService;

        public CreatePatientHandler(IApiResponseBuilder<CreatePatientResponse> builder,
            IValidator<CreatePatientCommand> validator,
            IMapper mapper,
            IPatientService patientService)
        {
            _builder = builder;
            _validator = validator;
            _mapper = mapper;
            _patientService = patientService;
        }

        public async Task<IApiResponse<CreatePatientResponse>> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            //validate
            var validation = await _validator.ValidateAsync(request, cancellationToken);
            if (validation.Errors.Any())
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var mappedPatient = _mapper.Map<PatientEntity>(request);

            var output = await _patientService.CreatePatient(mappedPatient, cancellationToken);

            var mappedResponse = _mapper.Map<CreatePatientResponse>(output);

            return _builder.CreateSuccessResponse(mappedResponse, x =>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Patient details were saved successfully");
            });
        }
    }
}
