using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Consultation.Domain.Entities
{
    [Table("DoctorAvailabilities")]
    public class DoctorAvailability
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required] public string DoctorId { get; set; }
        [Required] public string Date { get; set; }
        [Required] public DateTime TimeSlotStart { get; set; }
        [Required] public DateTime TimeSlotEnd { get; set; }
        public bool IsBooked { get; set; }
        public string BookedPatientId { get; set; }
        public DateTime? BookingDateTime { get; set; }
    }
}