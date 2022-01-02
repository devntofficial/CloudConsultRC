using AutoMapper;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Infrastructure.Mappers
{
    public class ConsultationMapper : Profile
    {
        public ConsultationMapper()
        {
            CreateMap<RequestConsultation, ConsultationRequest>();
            CreateMap<List<ConsultationRequest>, ConsultationResponse>().ConstructUsing(x => ConsultationResponseMapper(x));
            CreateMap<ConsultationRequest, GetConsultationByIdResponse>()
                .ForMember(x => x.ConsultationId, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.BookingDate, y => y.MapFrom(z => $"{z.TimeSlot.TimeSlotStart:dd-MM-yyyy}"))
                .ForMember(x => x.BookingTimeSlot, y => y.MapFrom(z => $"{z.TimeSlot.TimeSlotStart:HH:mm}-{z.TimeSlot.TimeSlotEnd:HH:mm}"));
        }

        private static ConsultationResponse ConsultationResponseMapper(List<ConsultationRequest> consultations)
        {
            if (consultations is null || consultations.Count == 0)
            {
                return new();
            }

            var doctor = consultations.Select(x => new { x.DoctorProfileId, x.DoctorName, x.DoctorEmailId, x.DoctorMobileNo }).First();

            return new ConsultationResponse
            {
                DoctorProfileId = doctor.DoctorProfileId,
                DoctorName = doctor.DoctorName,
                DoctorEmailId = doctor.DoctorEmailId,
                Consultations = consultations.Select(x => new ConsultationData
                {
                    Id = x.Id,
                    MemberProfileId = x.MemberProfileId,
                    MemberName = x.MemberName,
                    MemberEmailId = x.MemberEmailId,
                    BookingDate = $"{x.TimeSlot.TimeSlotStart:dd-MM-yyyy}",
                    BookingTimeSlot = $"{x.TimeSlot.TimeSlotStart:HH:mm}-{x.TimeSlot.TimeSlotEnd:HH:mm}",
                    Description = x.Description,
                    Status = x.Status
                }).ToList()
            };
        }
    }
}