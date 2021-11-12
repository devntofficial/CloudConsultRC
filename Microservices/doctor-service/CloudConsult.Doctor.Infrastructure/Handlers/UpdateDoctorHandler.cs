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
    public class UpdateDoctorHandler : ICommandHandler<UpdateDoctorCommand, UpdateDoctorResponse>
    {

        private readonly IApiResponseBuilder<UpdateDoctorResponse> _builder;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateDoctorCommand> _validator;
        

        public UpdateDoctorHandler(IApiResponseBuilder<UpdateDoctorResponse> builder,
            IDoctorService doctorService, IMapper mapper, IValidator<UpdateDoctorCommand> validator)
        {
            _builder = builder;
            _doctorService = doctorService;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<IApiResponse<UpdateDoctorResponse>> Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request, cancellationToken).ConfigureAwait(false);
            if(!validation.IsValid)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status400BadRequest);
                    x.WithErrors(validation.Errors.Select(y=>y.ErrorMessage));
                });
            }

            var mappedDoctor = _mapper.Map<UpdateDoctorCommand, DoctorEntity>(request);

            var updatedDoctor = await _doctorService.UpdateDoctor(mappedDoctor, cancellationToken);

            var response = _mapper.Map<UpdateDoctorResponse>(updatedDoctor);
            if(response == null)
            {
                return _builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                    x.WithErrors("Doctor with given Id not found!");
                });
            }
            
            return _builder.CreateSuccessResponse(response, x=>
            {
                x.WithSuccessCode(StatusCodes.Status200OK);
                x.WithMessages("Doctor updated successfully",
                    $"Updated details has been sent to{ request.EmailId}");
            });
        }
    }
}
