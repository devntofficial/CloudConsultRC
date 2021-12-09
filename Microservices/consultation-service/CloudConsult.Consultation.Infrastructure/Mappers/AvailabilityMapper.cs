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
            CreateMap<AddAvailability, List<DoctorAvailability>>()
                .ConvertUsing(new AvailabilityMapConverter());
            
            CreateMap<IEnumerable<DoctorAvailability>, DoctorAvailabilityResponse>()
                .ConvertUsing(new AvailabilityReverseMapConverter());
        }
    }

    public class
        AvailabilityReverseMapConverter : ITypeConverter<IEnumerable<DoctorAvailability>, DoctorAvailabilityResponse>
    {
        public DoctorAvailabilityResponse Convert(IEnumerable<DoctorAvailability> source,
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

    public class AvailabilityMapConverter : ITypeConverter<AddAvailability, List<DoctorAvailability>>
    {
        public List<DoctorAvailability> Convert(AddAvailability source, List<DoctorAvailability> destination,
            ResolutionContext context)
        {
            destination ??= new List<DoctorAvailability>();

            foreach (var map in source.AvailabilityMap)
            {
                destination.AddRange(map.Value.Select(x => new DoctorAvailability
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