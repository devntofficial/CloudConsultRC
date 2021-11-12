using AutoMapper;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Events;
using CloudConsult.Doctor.Domain.Responses;
using MongoDB.Bson;
using System;

namespace CloudConsult.Doctor.Infrastructure.Mappers
{
    public class DoctorMapper : Profile
    {
        public DoctorMapper()
        {
            CreateMap<CreateDoctorCommand, DoctorEntity>();
            CreateMap<DoctorEntity, CreateDoctorResponse>()
                .ForMember(x => x.DoctorId, y => y.MapFrom(z => z.Id.ToString()));

            CreateMap<UpdateDoctorCommand, DoctorEntity>()
                .ForMember(x => x.Id, y => y.MapFrom(z => ConvertStringToObjectId(z.DoctorId)));
            CreateMap<DoctorEntity, UpdateDoctorResponse>()
                .ForMember(x => x.DoctorId, y => y.MapFrom(z => z.Id.ToString()));

            CreateMap<DoctorEntity, DoctorCreatedEvent>()
                .ForMember(x => x.DoctorId, y => y.MapFrom(z => z.Id.ToString()));
            CreateMap<DoctorEntity, DoctorUpdatedEvent>()
                .ForMember(x => x.DoctorId, y => y.MapFrom(z => z.Id.ToString()));
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