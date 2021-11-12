using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Common.CQRS;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudConsult.Doctor.Domain.Commands
{
    public class UpdateDoctorCommand : ICommand<UpdateDoctorResponse>
    {
        public string DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
    }

    public class UpdateDoctorValidator : AbstractValidator<UpdateDoctorCommand>
    {
        public UpdateDoctorValidator()
        {

        }
    }
}
