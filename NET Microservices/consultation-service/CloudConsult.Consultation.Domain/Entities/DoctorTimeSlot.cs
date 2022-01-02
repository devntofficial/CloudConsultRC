using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Consultation.Domain.Entities;

[Table("DoctorTimeSlots")]
public class DoctorTimeSlot
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [Required] public string DoctorProfileId { get; set; }
    [Required] public DateTime TimeSlotStart { get; set; }
    [Required] public DateTime TimeSlotEnd { get; set; }

    public bool IsBooked { get; set; } = false;

    [ForeignKey("Consultation")]
    public string ConsultationId { get; set; } = null;
    public virtual ConsultationRequest Consultation { get; set; }
}