using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Identity.Domain.Entities
{
    [Table("UserOtp")]
    public class UserOtp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }

        [Required]
        public int Otp { get; set; }

        public DateTime ExpiryTimestamp { get; set; } = DateTime.UtcNow.AddMinutes(5);

        [Required]
        public bool IsEventPublished { get; set; } = false;

        [Required]

        public virtual User User { get; set; }
    }
}
