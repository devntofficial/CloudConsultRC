using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Consultation.Domain.Entities
{
    [Table("ConsultationBookings")]
    public class ConsultationBooking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Required]
        public string DoctorId { get; set; }
        
        [Required]
        public string PatentId { get; set; }
        
        [Required]
        public Guid TimeSlotId { get; set; }
        
        [Required]
        public DateTime BookingStartDateTime { get; set; }
        
        [Required]
        public DateTime BookingEndDateTime { get; set; }
        
        [Required]
        public bool IsBookingEventPublished { get; set; }
        
        [Required]
        public bool IsAcceptedByDoctor { get; set; }
        
        [Required]
        public bool IsPaymentComplete { get; set; }
        
        [Required]
        public bool IsDiagnosisReportGenerated { get; set; }
        
        public string DiagnosisReportId { get; set; }
        
        [Required]
        public bool IsConsultationComplete { get; set; }
    }
}