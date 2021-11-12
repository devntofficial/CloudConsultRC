using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudConsult.Identity.Domain.Entities
{
    [Table("Roles")]
    public class RoleEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public string RoleName { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }
    }
}