using AutoMapper;
using CloudConsult.Patient.Domain.Commands;
using CloudConsult.Patient.Domain.Entities;
using CloudConsult.Patient.Domain.Responses;

namespace CloudConsult.Patient.Infrastructure.Mappers
{
    public class PatientMapper : Profile
    {
        public PatientMapper()
        {
            CreateMap<CreatePatientCommand, PatientEntity>();
            CreateMap<PatientEntity, CreatePatientResponse>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id.ToString()));
        }
    }
}
