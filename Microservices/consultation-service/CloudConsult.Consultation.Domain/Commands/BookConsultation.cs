using CloudConsult.Common.CQRS;
using CloudConsult.Common.Validators;
using FluentValidation;
using System.Globalization;

namespace CloudConsult.Consultation.Domain.Commands
{
    public record BookConsultation : ICommand<String>
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string BookingDate { get; set; }
        public string BookingTimeSlot { get; set; }
    }

    public class BookConsultationCommandValidator : ApiValidator<BookConsultation>
    {
        public BookConsultationCommandValidator()
        {
            RuleFor(x => x.DoctorId).NotEmpty();
            RuleFor(x => x.DoctorName).NotEmpty();
            RuleFor(x => x.PatientId).NotEmpty();
            RuleFor(x => x.PatientName).NotEmpty();
            RuleFor(x => x.BookingDate).NotEmpty();
            RuleFor(x => x.BookingDate).Must(HaveValidDateFormat).WithMessage("Invalid BookingDate entered");
            RuleFor(x => x.BookingTimeSlot).NotEmpty();
            RuleFor(x => x.BookingTimeSlot).Must(HaveValidTimeslotFormat).WithMessage("Invalid BookingTimeSlot format");
        }



        private bool HaveValidTimeslotFormat(BookConsultation parent, string timeSlot)
        {
            var slot = timeSlot.Split("-");
            if (slot.Length != 2)
            {
                return false;
            }

            if (!DateTime.TryParseExact($"{parent.BookingDate} {slot[0]}", "dd-MM-yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var startTime))
            {
                return false;
            }

            return DateTime.TryParseExact($"{parent.BookingDate} {slot[1]}", "dd-MM-yyyy HH:mm",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out var endTime);
        }
    }
}