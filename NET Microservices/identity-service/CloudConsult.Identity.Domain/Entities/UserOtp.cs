using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Identity.Domain.Entities
{
    [Table("UserOtp")]
    public class UserOtp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public int Otp { get; set; }

        public DateTime ExpiryTimestamp { get; set; } = DateTime.Now.AddMinutes(5);

        [Required]
        public bool IsEventPublished { get; set; } = false;

        [Required]

        public virtual User User { get; set; }
    }
}
