using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Infrastructure.Mappers
{
    public class AvailabilityMapper : Profile
    {
        public AvailabilityMapper()
        {
            CreateMap<AddAvailabilityCommand, List<DoctorAvailabilityEntity>>()
                .ConvertUsing(new AvailabilityMapConverter());
            
            CreateMap<IEnumerable<DoctorAvailabilityEntity>, DoctorAvailabilityResponse>()
                .ConvertUsing(new AvailabilityReverseMapConverter());
        }
    }

    public class
        AvailabilityReverseMapConverter : ITypeConverter<IEnumerable<DoctorAvailabilityEntity>, DoctorAvailabilityResponse>
    {
        public DoctorAvailabilityResponse Convert(IEnumerable<DoctorAvailabilityEntity> source,
            DoctorAvailabilityResponse destination,
            ResolutionContext context)
        {
            destination ??= new DoctorAvailabilityResponse();
            var doctorAvailabilities = source.ToList();
            destination.DoctorId = doctorAvailabilities.First().DoctorId;
            destination.AvailabilityMap = doctorAvailabilities
                .GroupBy(k => k.Date, v => $"{v.TimeSlotStart:HH:mm}-{v.TimeSlotEnd:HH:mm}")
                .ToDictionary(k => k.Key, v => v.ToList());
            return destination;
        }
    }

    public class AvailabilityMapConverter : ITypeConverter<AddAvailabilityCommand, List<DoctorAvailabilityEntity>>
    {
        public List<DoctorAvailabilityEntity> Convert(AddAvailabilityCommand source, List<DoctorAvailabilityEntity> destination,
            ResolutionContext context)
        {
            destination ??= new List<DoctorAvailabilityEntity>();

            foreach (var map in source.AvailabilityMap)
            {
                destination.AddRange(map.Value.Select(x => new DoctorAvailabilityEntity
                {
                    DoctorId = source.DoctorId,
                    Date = map.Key,
                    TimeSlotStart = DateTime.ParseExact($"{map.Key} {x.Split("-")[0]}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    TimeSlotEnd = DateTime.ParseExact($"{map.Key} {x.Split("-")[1]}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)
                }));
            }

            return destination;
        }
    }
}