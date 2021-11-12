using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Identity.Domain.Entities
{
    [Table("UserRoles")]
    public sealed class UserRoleEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public Guid UserId { get; set; }
        
        public int RoleId { get; set; }
        
        public DateTime Timestamp { get; set; }

        public RoleEntity Role { get; set; }
        public UserEntity User { get; set; }
    }
}