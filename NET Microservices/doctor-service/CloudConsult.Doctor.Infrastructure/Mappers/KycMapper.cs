using AutoMapper;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Events;

namespace CloudConsult.Doctor.Infrastructure.Mappers
{
    public class KycMapper : Profile
    {
        public KycMapper()
        {
            CreateMap<DoctorKyc, KycApproved>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id));
            CreateMap<DoctorKyc, KycRejected>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id));
        }
    }
}
