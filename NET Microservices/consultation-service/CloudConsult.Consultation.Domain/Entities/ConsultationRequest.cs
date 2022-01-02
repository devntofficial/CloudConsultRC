using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Consultation.Domain.Entities;

[Table("ConsultationRequests")]
public class ConsultationRequest
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [Required]
    public string DoctorProfileId { get; set; }

    [Required]
    public string DoctorName { get; set; }

    [Required]
    public string DoctorEmailId { get; set; }

    [Required]
    public string DoctorMobileNo { get; set; }

    [Required]
    public string MemberProfileId { get; set; }

    [Required]
    public string MemberName { get; set; }

    [Required]
    public string MemberEmailId { get; set; }

    [Required]
    public string MemberMobileNo { get; set; }

    [Required]
    [ForeignKey("TimeSlot")]
    public string TimeSlotId { get; set; }
    public virtual DoctorTimeSlot TimeSlot { get; set; }

    [Required]
    public string Description { get; set; }

    public string PaymentId { get; set; }
    public string DiagnosisReportId { get; set; }

    [Required]
    public bool IsComplete { get; set; }

    [Required]
    public string Status { get; set; }

    public virtual ICollection<ConsultationEvent> Events { get; set; }
}