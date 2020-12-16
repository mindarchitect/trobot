using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRobot.Core.Data.Entities
{
    [Table("Users")]
    public class UserEntity : Entity
    {
        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [ForeignKey("RoleId")]
        public ICollection<RoleEntity> Roles { get; set; }
    }
}
