using System;
using System.Globalization;
using System.Linq.Expressions;
using AutoMapper;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Events;

namespace CloudConsult.Consultation.Infrastructure.Mappers
{
    public class ConsultationMapper: Profile
    {
        public ConsultationMapper()
        {
            CreateMap<BookConsultation, ConsultationBooking>()
                .ForMember(x => x.BookingStartDateTime,
                    y => y.MapFrom(z => BookingStartDateMapper(z)))
                .ForMember(x => x.BookingEndDateTime,
                    y => y.MapFrom(z => BookingEndDateMapper(z)));
            
            CreateMap<ConsultationBooking, ConsultationBooked>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id.ToString()))
                .ForMember(x => x.BookingDate, y =>
                    y.MapFrom(z => $"{z.BookingStartDateTime:dd-MM-yyyy}"))
                .ForMember(x => x.BookingTimeSlot,
                    y => y.MapFrom(z => $"{z.BookingStartDateTime:HH:mm}-{z.BookingEndDateTime:HH:mm}"));
        }

        private DateTime BookingEndDateMapper(BookConsultation data)
        {
            var endTime = data.BookingTimeSlot.Split("-")[1];
            return DateTime.ParseExact($"{data.BookingDate} {endTime}", "dd-MM-yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        private DateTime BookingStartDateMapper(BookConsultation data)
        {
            var startTime = data.BookingTimeSlot.Split("-")[0];
            return DateTime.ParseExact($"{data.BookingDate} {startTime}", "dd-MM-yyyy HH:mm",
                    CultureInfo.InvariantCulture, DateTimeStyles.None);
        }
    }
}