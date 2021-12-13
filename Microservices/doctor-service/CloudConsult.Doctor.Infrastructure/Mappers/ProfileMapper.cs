using AutoMapper;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Events;
using CloudConsult.Doctor.Domain.Responses;

namespace CloudConsult.Doctor.Infrastructure.Mappers
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<CreateProfile, DoctorProfile>();
            CreateMap<DoctorProfile, ProfileResponse>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id));

            CreateMap<UpdateProfile, DoctorProfile>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.ProfileId));
            CreateMap<DoctorProfile, ProfileResponse>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id));

            CreateMap<DoctorProfile, ProfileCreated>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id));
            CreateMap<DoctorProfile, ProfileUpdated>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id));

            CreateMap<DoctorKyc, KycApproved>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id));
            CreateMap<DoctorKyc, KycRejected>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id));
        }
    }
}