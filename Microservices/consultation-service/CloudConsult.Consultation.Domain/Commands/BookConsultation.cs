using System;
using System.Globalization;
using CloudConsult.Common.CQRS;
using FluentValidation;

namespace CloudConsult.Consultation.Domain.Commands
{
    public record BookConsultation : ICommand<String>
    {
        public string DoctorId { get; set; }
        public string PatentId { get; set; }
        public string BookingDate { get; set; }
        public string BookingTimeSlot { get; set; }
    }
    
    public class BookConsultationCommandValidator : AbstractValidator<BookConsultation>
    {
        public BookConsultationCommandValidator()
        {
            RuleFor(x => x.DoctorId).NotEmpty();
            
            RuleFor(x => x.PatentId).NotEmpty();
            
            RuleFor(x => x.BookingDate).NotEmpty()
                .Must(HaveValidDateFormat)
                .WithMessage("Invalid BookingDate entered");
            
            RuleFor(x => x.BookingTimeSlot).NotEmpty()
                .Must(HaveValidTimeslotFormat)
                .WithMessage("Invalid BookingTimeSlot format");
        }

        private bool HaveValidDateFormat(string date)
        {
            return DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var outDate);
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