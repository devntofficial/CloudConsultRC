using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Consultation.Domain.Entities
{
    public class ConsultationEvent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [ForeignKey("Consultation")]
        public string ConsultationId { get; set; }
        public ConsultationRequest Consultation { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public bool IsEventPublished { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
