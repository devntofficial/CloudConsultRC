using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Consultation.Domain.Entities;

[Table("ConsultationBookings")]
public class ConsultationBooking
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string DoctorProfileId { get; set; }

    [Required]
    public string DoctorName { get; set; }

    [Required]
    public string DoctorEmailId { get; set; }

    [Required]
    public string PatientProfileId { get; set; }

    [Required]
    public string PatientName { get; set; }

    [Required]
    public string PatientEmailId { get; set; }

    [Required]
    public Guid TimeSlotId { get; set; }

    [Required]
    public DateTime BookingStartDateTime { get; set; }

    [Required]
    public DateTime BookingEndDateTime { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public string Status { get; set; }

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