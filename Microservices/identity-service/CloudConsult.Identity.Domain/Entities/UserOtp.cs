using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Identity.Domain.Entities
{
    [Table("UserOtp")]
    public sealed class UserOtp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public int Otp { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime ExpiryTimestamp { get; set; }
    }
}
