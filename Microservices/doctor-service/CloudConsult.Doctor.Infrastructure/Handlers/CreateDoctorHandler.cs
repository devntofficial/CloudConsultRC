using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Doctor.Domain.Services;
using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Doctor.Infrastructure.Handlers
{
    public class CreateDoctorHandler : ICommandHandler<CreateDoctorCommand, CreateDoctorResponse>
    {
        private readonly IApiResponseBuilder<CreateDoctorResponse> _builder;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateDoctorCommand> _validator;

        public CreateDoctorHandler(IApiResponseBuilder<CreateDoctorResponse> builder,
            IDoctorService doctorService, IMapper mapper, IValidator<CreateDoctorCommand> validator)
        {
            _builder = builder;
            _doctorService = doctorService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IApiResponse<CreateDoctorResponse>> Handle(CreateDoctorCommand request,
            CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
            if (!validation.IsValid)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y => y.ErrorMessage));
                });
            }

            var mappedDoctor = _mapper.Map<DoctorEntity>(request);

            var createdDoctor = await _doctorService.Create(mappedDoctor, cancellationToken).ConfigureAwait(false);

            var response = _mapper.Map<CreateDoctorResponse>(createdDoctor);

            return _builder.CreateSuccessResponse(response, x =>
            {
                x.WithSuccessCode(StatusCodes.Status201Created);
                x.WithMessages("Doctor created successfully",
                    $"A verification email has been sent to {request.EmailId}");
            });
        }
    }
}