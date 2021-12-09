using AutoMapper;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Events;
using CloudConsult.Consultation.Domain.Responses;
using System.Globalization;

namespace CloudConsult.Consultation.Infrastructure.Mappers
{
    public class ConsultationMapper : Profile
    {
        public ConsultationMapper()
        {
            CreateMap<BookConsultation, ConsultationBooking>()
                .ForMember(x => x.BookingStartDateTime,
                    y => y.MapFrom(z => BookingStartDateMapper(z)))
                .ForMember(x => x.BookingEndDateTime,
                    y => y.MapFrom(z => BookingEndDateMapper(z)));

            CreateMap<ConsultationBooking, ConsultationBooked>()
                .ForMember(x => x.BookingDate, y =>
                    y.MapFrom(z => $"{z.BookingStartDateTime:dd-MM-yyyy}"))
                .ForMember(x => x.BookingTimeSlot,
                    y => y.MapFrom(z => $"{z.BookingStartDateTime:HH:mm}-{z.BookingEndDateTime:HH:mm}"));

            CreateMap<List<ConsultationBooking>, ConsultationResponse>().ConvertUsing(x => ConsultationResponseMapper(x));
        }

        private ConsultationResponse ConsultationResponseMapper(List<ConsultationBooking> consultations)
        {
            if(consultations is null || consultations.Count == 0)
            {
                return new();
            }

            var doctor = consultations.Select(x => new { x.DoctorProfileId, x.DoctorName, x.DoctorEmailId }).First();

            return new ConsultationResponse
            {
                DoctorProfileId = doctor.DoctorProfileId,
                DoctorName = doctor.DoctorName,
                DoctorEmailId = doctor.DoctorEmailId,
                Consultations = consultations.Select(x => new ConsultationData
                {
                    Id = x.Id.ToString(),
                    PatientProfileId = x.PatientProfileId,
                    PatientName = x.PatientName,
                    PatientEmailId = x.PatientEmailId,
                    BookingDate = $"{x.BookingStartDateTime:dd-MM-yyyy}",
                    BookingTimeSlot = $"{x.BookingStartDateTime:HH:mm}-{x.BookingEndDateTime:HH:mm}",
                    Description = x.Description,
                    Status = x.Status,
                    DiagnosisReportId = x.DiagnosisReportId,
                    IsAcceptedByDoctor = x.IsAcceptedByDoctor,
                    IsConsultationComplete = x.IsConsultationComplete,
                    IsDiagnosisReportGenerated = x.IsDiagnosisReportGenerated,
                    IsPaymentComplete = x.IsPaymentComplete,
                }).ToList()
            };
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