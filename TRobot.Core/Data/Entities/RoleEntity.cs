using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TRobot.Core.Data.Entities
{
    [Table("Roles")]
    public class RoleEntity : Entity
    {
        [Required]
        public string Name { get; set; }

        [ForeignKey("UserId")]
        public ICollection<UserEntity> Users { get; set; }
    }
}
