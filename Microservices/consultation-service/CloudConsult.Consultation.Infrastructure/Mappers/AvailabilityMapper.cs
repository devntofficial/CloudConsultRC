﻿using AutoMapper;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;
using System.Globalization;

namespace CloudConsult.Consultation.Infrastructure.Mappers
{
    public class AvailabilityMapper : Profile
    {
        public AvailabilityMapper()
        {
            CreateMap<AddTimeSlot, List<DoctorTimeSlot>>().ConvertUsing(new AvailabilityMapConverter());

            CreateMap<IEnumerable<DoctorTimeSlot>, TimeSlotResponse>().ConvertUsing(new AvailabilityReverseMapConverter());
        }
    }

    public class
        AvailabilityReverseMapConverter : ITypeConverter<IEnumerable<DoctorTimeSlot>, TimeSlotResponse>
    {
        public TimeSlotResponse Convert(IEnumerable<DoctorTimeSlot> source,
            TimeSlotResponse destination,
            ResolutionContext context)
        {
            destination ??= new TimeSlotResponse();
            var doctorAvailabilities = source.ToList();
            destination.DoctorId = doctorAvailabilities.First().DoctorProfileId;
            destination.AvailabilityMap = doctorAvailabilities
                .GroupBy(k => k.TimeSlotStart.Date, v => $"{v.TimeSlotStart:HH:mm}-{v.TimeSlotEnd:HH:mm}")
                .ToDictionary(k => k.Key.ToString("dd-MM-yyyy"), v => v.ToList());
            return destination;
        }
    }

    public class AvailabilityMapConverter : ITypeConverter<AddTimeSlot, List<DoctorTimeSlot>>
    {
        public List<DoctorTimeSlot> Convert(AddTimeSlot source, List<DoctorTimeSlot> destination,
            ResolutionContext context)
        {
            destination ??= new List<DoctorTimeSlot>();

            foreach (var map in source.AvailabilityMap)
            {
                destination.AddRange(map.Value.Select(x => new DoctorTimeSlot
                {
                    DoctorProfileId = source.DoctorProfileId,
                    TimeSlotStart = DateTime.ParseExact($"{map.Key} {x.Split("-")[0]}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture),
                    TimeSlotEnd = DateTime.ParseExact($"{map.Key} {x.Split("-")[1]}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture)
                }));
            }

            return destination;
        }
    }
}