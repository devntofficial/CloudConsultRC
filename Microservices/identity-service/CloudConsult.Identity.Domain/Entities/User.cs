using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Identity.Domain.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string EmailId { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsVerified { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual UserOtp CurrentOtp { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}