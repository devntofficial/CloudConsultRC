using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Identity.Domain.Entities
{
    [Table("UserRoles")]
    public sealed class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string UserId { get; set; }
        
        public int RoleId { get; set; }
        
        public DateTime Timestamp { get; set; }

        public Role Role { get; set; }
        public User User { get; set; }
    }
}