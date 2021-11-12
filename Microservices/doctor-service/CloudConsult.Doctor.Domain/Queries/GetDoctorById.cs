using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Common.CQRS;

namespace CloudConsult.Doctor.Domain.Queries
{
    public record GetDoctorByIdQuery : IQuery<CreateDoctorResponse>
    {
        public string DoctorId { get; set; }
    }
}