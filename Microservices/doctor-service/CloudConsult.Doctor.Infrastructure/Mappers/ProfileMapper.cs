using AutoMapper;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Events;
using CloudConsult.Doctor.Domain.Responses;
using MongoDB.Bson;
using System;

namespace CloudConsult.Doctor.Infrastructure.Mappers
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<CreateProfile, DoctorProfile>();
            CreateMap<DoctorProfile, ProfileResponse>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));

            CreateMap<UpdateProfile, DoctorProfile>()
                .ForMember(x => x.Id, y => y.MapFrom(z => ConvertStringToObjectId(z.ProfileId)));
            CreateMap<DoctorProfile, ProfileResponse>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));

            CreateMap<DoctorProfile, ProfileCreated>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));
            CreateMap<DoctorProfile, ProfileUpdated>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));

            CreateMap<DoctorKyc, KycApproved>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));
            CreateMap<DoctorKyc, KycRejected>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));
        }

        private object ConvertStringToObjectId(string doctorId)
        {
            if(ObjectId.TryParse(doctorId, out var convertedValue))
            {
                return convertedValue;
            }
            return ObjectId.Empty;
        }
    }
}