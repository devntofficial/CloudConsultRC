using AutoMapper;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Events;

namespace CloudConsult.Consultation.Infrastructure.Mappers
{
    public class EventMapper : Profile
    {
        public EventMapper()
        {
            CreateMap<ConsultationEvent, ConsultationRequested>().ConstructUsing(x => ConsultationRequestedMapper(x));
            CreateMap<ConsultationEvent, ConsultationAccepted>().ConstructUsing(x => ConsultationAcceptedMapper(x));
            CreateMap<ConsultationEvent, ConsultationRejected>().ConstructUsing(x => ConsultationRejectedMapper(x));
            CreateMap<ConsultationEvent, ConsultationCancelled>().ConstructUsing(x => ConsultationCancelledMapper(x));
        }

        private ConsultationRequested ConsultationRequestedMapper(ConsultationEvent data)
        {
            var consultation = data.Consultation;
            return new ConsultationRequested
            {
                EventId = data.Id,
                ConsultationId = consultation.Id,
                DoctorProfileId = consultation.DoctorProfileId,
                DoctorEmailId = consultation.DoctorEmailId,
                DoctorMobileNo = consultation.DoctorMobileNo,
                DoctorName = consultation.DoctorName,
                MemberEmailId = consultation.MemberEmailId,
                MemberMobileNo = consultation.MemberMobileNo,
                MemberName = consultation.MemberName,
                MemberProfileId = consultation.MemberProfileId,
                BookingDate = $"{consultation.TimeSlot.TimeSlotStart:dd-MM-yyyy}",
                BookingTimeSlot = $"{consultation.TimeSlot.TimeSlotStart:HH:mm}-{consultation.TimeSlot.TimeSlotEnd:HH:mm}",
                TimeSlotId = consultation.TimeSlotId,
                Description = consultation.Description
            };
        }

        private ConsultationAccepted ConsultationAcceptedMapper(ConsultationEvent data)
        {
            var consultation = data.Consultation;
            return new ConsultationAccepted
            {
                EventId = data.Id,
                ConsultationId = consultation.Id,
                DoctorProfileId = consultation.DoctorProfileId,
                DoctorEmailId = consultation.DoctorEmailId,
                DoctorMobileNo = consultation.DoctorMobileNo,
                DoctorName = consultation.DoctorName,
                MemberEmailId = consultation.MemberEmailId,
                MemberMobileNo = consultation.MemberMobileNo,
                MemberName = consultation.MemberName,
                MemberProfileId = consultation.MemberProfileId
            };
        }

        private ConsultationRejected ConsultationRejectedMapper(ConsultationEvent data)
        {
            var consultation = data.Consultation;
            return new ConsultationRejected
            {
                EventId = data.Id,
                ConsultationId = consultation.Id,
                DoctorProfileId = consultation.DoctorProfileId,
                DoctorEmailId = consultation.DoctorEmailId,
                DoctorMobileNo = consultation.DoctorMobileNo,
                DoctorName = consultation.DoctorName,
                MemberEmailId = consultation.MemberEmailId,
                MemberMobileNo = consultation.MemberMobileNo,
                MemberName = consultation.MemberName,
                MemberProfileId = consultation.MemberProfileId
            };
        }

        private ConsultationCancelled ConsultationCancelledMapper(ConsultationEvent data)
        {
            var consultation = data.Consultation;
            return new ConsultationCancelled
            {
                EventId = data.Id,
                ConsultationId = consultation.Id,
                DoctorProfileId = consultation.DoctorProfileId,
                DoctorEmailId = consultation.DoctorEmailId,
                DoctorMobileNo = consultation.DoctorMobileNo,
                DoctorName = consultation.DoctorName,
                MemberEmailId = consultation.MemberEmailId,
                MemberMobileNo = consultation.MemberMobileNo,
                MemberName = consultation.MemberName,
                MemberProfileId = consultation.MemberProfileId
            };
        }
    }
}
